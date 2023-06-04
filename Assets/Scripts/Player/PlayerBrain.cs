using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using InputReader;

namespace Player
{
    public class PlayerBrain : IDisposable
    {
        private readonly PlayerEntityHandler _player;
        private readonly List<IEntityInputSource> _inputSources;

        
        public PlayerBrain(PlayerEntityHandler player, List<IEntityInputSource> inputSources)
        {
            _player = player;
            _inputSources = inputSources;
            
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        public void Dispose() => ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;

        private void OnFixedUpdate()
        {
            _player.MoveHorizontally(GetDirection());

            if (IsJumping)
                _player.Jump();

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