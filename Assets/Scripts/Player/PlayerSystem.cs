using System;
using System.Collections.Generic;
using System.Linq;
using Core.Parallax;
using InputReader;
using StatsSystem;
using StatsSystem.Storages;
using UnityEngine;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly PlayerEntity _playerEntity;
        private readonly List<IDisposable> _disposables;

        public StatsController StatsController { get; }

        
        public PlayerSystem(PlayerEntityBehaviour playerBehaviour, IParallaxTargetMovement parallaxTargetMovement, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            StatsController = new StatsController(stats);
            _disposables.Add(StatsController);

            _playerEntity = new PlayerEntity(playerBehaviour, StatsController, parallaxTargetMovement, inputSources);
            _disposables.Add(_playerEntity);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}