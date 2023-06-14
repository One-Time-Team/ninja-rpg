using System;
using System.Collections.Generic;
using System.Linq;
using Core.Parallax;
using InputReader;
using NPC.Controller;
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
        public bool IsPlayerDead { get; private set; }

        
        public PlayerSystem(PlayerEntityBehaviour playerBehaviour, IParallaxTargetMovement parallaxTargetMovement, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statsStorage = Resources.Load<StatsStorage>($"{nameof(Player)}/{nameof(StatsStorage)}");
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            StatsController = new StatsController(stats);
            _disposables.Add(StatsController);

            _playerEntity = new PlayerEntity(playerBehaviour, StatsController, parallaxTargetMovement, inputSources);
            _disposables.Add(_playerEntity);
            
            _playerEntity.Died += SetDead;
        }

        public void Dispose()
        {
            _playerEntity.Died -= SetDead;
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }

        private void SetDead(Entity entity)
        {
            IsPlayerDead = true;
        }
    }
}