using System;
using System.Collections.Generic;
using System.Linq;
using Core.Parallax;
using Core.Services.Updater;
using InputReader;
using NPC.Behaviour;
using NPC.Controller;
using StatsSystem.Data;
using StatsSystem.Enums;
using UnityEngine;

namespace Player
{
    public class PlayerEntity : Entity, IDisposable
    {
        private readonly PlayerEntityBehaviour _player;
        private readonly List<IEntityInputSource> _inputSources;


        public PlayerEntity(PlayerEntityBehaviour player, IStatValueGiver statValueGiver, IParallaxTargetMovement parallaxPlayerMovement, 
            List<IEntityInputSource> inputSources) : base(player, statValueGiver, parallaxPlayerMovement)
        {
            _player = player;
            _inputSources = inputSources;

            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
            VisualiseHP(StatValueGiver.GetValue(StatsSystem.Enums.StatType.Health));
            _player.Animator.ActionRequested += OnAttacked;
            Died += OnDeath;
        }

        public void Dispose() 
        { 
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
            _player.Animator.ActionRequested -= OnAttacked;
        }

        private void OnFixedUpdate()
        {
            _player.MoveHorizontally(GetDirection() * StatValueGiver.GetValue(StatType.Speed) * ParallaxPlayerMovement.ParallaxSpeedCoef);

            if (IsJumping)
                _player.Jump(StatValueGiver.GetValue(StatType.JumpForce));

            if (IsAttacking)
                _player.StartAttack();

            foreach (var inputSource in _inputSources)
                inputSource.ResetOneTimeActions();
        }

        private float GetDirection()
        {
            foreach (var inputSource in _inputSources)
            {
                if (inputSource.HorizontalDirection == 0)
                    continue;

                return inputSource.HorizontalDirection;
            }

            return 0;
        }

        private void OnAttacked()
        {
            if (_player.TryGetAttackTarget(out BaseEntityBehaviour target))
                target.TakeDamage(StatValueGiver.GetValue(StatsSystem.Enums.StatType.Damage));
        }

        protected sealed override void VisualiseHP(float currentHP)
        {
            if (_player.HPBar.maxValue < currentHP)
                _player.HPBar.maxValue = currentHP;

            _player.HPBar.value = currentHP;
        }

        private void OnDeath(Entity entity)
        {
            Dispose();
            _player.Die();
        }

        private bool IsJumping => _inputSources.Any(inputSource => inputSource.IsJumping);
        
        private bool IsAttacking => _inputSources.Any(inputSource => inputSource.IsAttacking);
    }
}