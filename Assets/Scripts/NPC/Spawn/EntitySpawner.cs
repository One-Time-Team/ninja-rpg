using System;
using System.Collections.Generic;
using Core.Parallax;
using NPC.Controller;
using NPC.Enums;
using NPC.Storages;
using UnityEngine;

namespace NPC.Spawn
{
    public class EntitySpawner : IDisposable
    {
        private readonly List<Entity> _entities;
        private readonly EntityFactory _entityFactory;

        public Transform Transform { get; private set; }
        
        public EntitySpawner(IParallaxTargetMovement parallaxTargetMovement)
        {
            _entities = new List<Entity>();
            var entitiesSpawnerDataStorage = Resources.Load<EntitiesSpawnerDataStorage>($"{nameof(EntitySpawner)}/{nameof(EntitiesSpawnerDataStorage)}");
            _entityFactory = new EntityFactory(entitiesSpawnerDataStorage, parallaxTargetMovement);
            Transform = _entityFactory.EntitiesContainer;
        }
        
        public void SpawnEntity(EntityId entityId, Vector2 position)
        {
            var entity = _entityFactory.GetEntityBrain(entityId, position);
            entity.Died += RemoveEntity;
            _entities.Add(entity);
        }

        public void Dispose()
        {
            foreach(var entity in _entities)
                DestroyEntity(entity);
            _entities.Clear();
        }

        private void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
            DestroyEntity(entity);
        }

        private void DestroyEntity(Entity entity)
        {
            entity.Died -= RemoveEntity;
        }
    }
}