using System;
using System.Collections.Generic;
using System.Linq;
using InputReader;
using StatsSystem;
using StatsSystem.Storages;
using UnityEngine;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly PlayerEntityHandler _player;
        private readonly PlayerBrain _playerBrain;
        private readonly List<IDisposable> _disposables;

        public StatsController StatsController { get; }

        
        public PlayerSystem(PlayerEntityHandler player, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            StatsController = new StatsController(stats);
            _disposables.Add(StatsController);
            
            _player = player;
            _player.Initialize(StatsController);
            
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