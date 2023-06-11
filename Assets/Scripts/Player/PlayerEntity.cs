using System.Collections.Generic;
using System.Linq;
using Core.Parallax;
using Core.Services.Updater;
using InputReader;
using NPC.Behaviour;
using NPC.Controller;
using StatsSystem.Data;
using StatsSystem.Enums;

namespace Player
{
    public class PlayerEntity : Entity
    {
        private readonly PlayerEntityBehaviour _player;
        private readonly List<IEntityInputSource> _inputSources;


        public PlayerEntity(PlayerEntityBehaviour player, IStatValueGiver statValueGiver, IParallaxTargetMovement parallaxPlayerMovement, 
            List<IEntityInputSource> inputSources) : base(player, statValueGiver, parallaxPlayerMovement)
        {
            _player = player;
            _inputSources = inputSources;

            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
            VisualiseHP(StatValueGiver.GetValue(StatType.Health));
            _player.AttackImpacted += OnAttack;
        }

        public override void Dispose() 
        { 
            base.Dispose();
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
            _player.AttackImpacted -= OnAttack;
        }
        
        protected sealed override void VisualiseHP(float currentHP)
        {
            if (_player.HPBar.maxValue < currentHP)
                _player.HPBar.maxValue = currentHP;

            _player.HPBar.value = currentHP;
        }

        private void OnFixedUpdate()
        {
            if (_player.IsAttackProcessing)
            {
                _player.MoveHorizontally(0);
                return;
            }
            
            _player.MoveHorizontally(GetDirection() * StatValueGiver.GetValue(StatType.Speed) * ParallaxPlayerMovement.ParallaxSpeedCoef);

            if (Jumped)
                _player.Jump(StatValueGiver.GetValue(StatType.JumpForce));

            if (AttackStarted)
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

        private void OnAttack()
        {
            if (_player.TryGetAttackTarget(out BaseEntityBehaviour target))
                target.TakeDamage(StatValueGiver.GetValue(StatType.Damage));
        }

        private bool Jumped => _inputSources.Any(inputSource => inputSource.Jumped);
        
        private bool AttackStarted => _inputSources.Any(inputSource => inputSource.AttackStarted);
    }
}