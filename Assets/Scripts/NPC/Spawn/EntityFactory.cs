using System;
using System.Linq;
using Core.Parallax;
using NPC.Behaviour;
using NPC.Controller;
using NPC.Enums;
using NPC.Storages;
using StatsSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NPC.Spawn
{
    public class EntityFactory
    {
        private readonly EntitiesSpawnerDataStorage _entitiesSpawnerDataStorage;
        private readonly IParallaxTargetMovement _parallaxTargetMovement;

        public Transform EntitiesContainer { get; private set; }
        
        public EntityFactory(EntitiesSpawnerDataStorage entitiesSpawnerDataStorage, IParallaxTargetMovement parallaxTargetMovement)
        {
            _entitiesSpawnerDataStorage = entitiesSpawnerDataStorage;
            _parallaxTargetMovement = parallaxTargetMovement;
            var gameObject = new GameObject
            {
                name = nameof(EntitySpawner)
            };
            EntitiesContainer = gameObject.transform;
        }

        public Entity GetEntityBrain(EntityId entityId, Vector2 position)
        {
            var data = _entitiesSpawnerDataStorage.EntitiesSpawnData.Find(element => element.Id == entityId);
            var baseEntityBehaviour = Object.Instantiate(data.EntityBehaviourPrefab, position, Quaternion.identity);
            baseEntityBehaviour.transform.SetParent(EntitiesContainer);
            var stats = data.Stats.Select(stat => stat.GetCopy()).ToList();
            var statsController = new StatsController(stats);
            switch (entityId)
            {
                case EntityId.Ronin:
                    return new MeleeEntity(baseEntityBehaviour as MeleeEntityBehaviour, statsController, _parallaxTargetMovement);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}