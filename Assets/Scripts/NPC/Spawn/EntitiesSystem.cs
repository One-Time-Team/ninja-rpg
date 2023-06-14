using System;
using System.Collections.Generic;
using System.Linq;
using Core.Parallax;
using ItemsSystem;
using NPC.Behaviour;
using NPC.Controller;
using NPC.Enums;
using NPC.Storages;
using StatsSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NPC.Spawn
{
    public class EntitiesSystem : IDisposable
    {
        private readonly List<Entity> _entities;
        private readonly EntitiesSpawnerDataStorage _entitiesSpawnerDataStorage;
        private readonly IParallaxTargetMovement _parallaxTargetMovement;
        
        public Transform Transform { get; }
        public bool AreEnemiesDead { get; private set; }
        
        
        public EntitiesSystem(IParallaxTargetMovement parallaxTargetMovement)
        {
            _parallaxTargetMovement = parallaxTargetMovement;
            _entitiesSpawnerDataStorage = Resources.Load<EntitiesSpawnerDataStorage>($"{nameof(EntitiesSystem)}/{nameof(EntitiesSpawnerDataStorage)}");
            _entities = new List<Entity>();
            
            var gameObject = new GameObject
            {
                name = nameof(EntitiesSystem)
            };
            Transform = gameObject.transform;
        }
        
        public void SpawnEntity(EntityId entityId, Vector2 position, DropGenerator dropGenerator)
        {
            var entity = GetEntityBrain(entityId, position, dropGenerator);
            entity.Died += RemoveEntity;
            _entities.Add(entity);
        }

        public void Dispose()
        {
            foreach (var entity in _entities)
            {
                entity.Died -= RemoveEntity;
                entity.Dispose();
            }
            _entities.Clear();
        }
        
        private Entity GetEntityBrain(EntityId entityId, Vector2 position, DropGenerator dropGenerator)
        {
            var data = _entitiesSpawnerDataStorage.EntitiesSpawnData.Find(element => element.Id == entityId);
            var baseEntityBehaviour = Object.Instantiate(data.EntityBehaviourPrefab, position, Quaternion.identity);
            baseEntityBehaviour.transform.SetParent(Transform);
            var stats = data.Stats.Select(stat => stat.GetCopy()).ToList();
            var statsController = new StatsController(stats);
            var dropAmount = data.DropAmount;
            switch (entityId)
            {
                case EntityId.Ronin:
                    return new MeleeEntity(baseEntityBehaviour as MeleeEntityBehaviour, statsController, _parallaxTargetMovement, dropGenerator, dropAmount);
                default:
                    throw new NotImplementedException();
            }
        }

        private void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
            entity.Died -= RemoveEntity;

            if (_entities.Count == 0)
                AreEnemiesDead = true;
        }
    }
}