using System;
using System.Collections.Generic;
using System.Linq;
using InputReader;
using StatsSystem;
using UnityEngine;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly PlayerEntityHandler _player;
        private readonly PlayerBrain _playerBrain;
        private readonly StatsController _statsController;
        private readonly List<IDisposable> _disposables;

        public PlayerSystem(PlayerEntityHandler player, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            _statsController = new StatsController(stats);
            _disposables.Add(_statsController);
            
            _player = player;
            _player.Initialize(_statsController);
            
            _playerBrain = new PlayerBrain(player, inputSources);
            _disposables.Add(_playerBrain);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}