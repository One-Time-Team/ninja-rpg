using System;
using System.Collections.Generic;
using System.Linq;
using Core.Parallax;
using Core.Services.Updater;
using InputReader;
using NPC.Controller;
using StatsSystem.Data;
using StatsSystem.Enums;

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
        }

        public void Dispose() => ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;

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

        private bool IsJumping => _inputSources.Any(inputSource => inputSource.IsJumping);
        
        private bool IsAttacking => _inputSources.Any(inputSource => inputSource.IsAttacking);
    }
}