using System.Collections.Generic;
using NPC.Behaviour;
using NPC.Enums;
using StatsSystem.Data;
using UnityEngine;

namespace NPC.Storages
{
    [CreateAssetMenu(fileName = nameof(EntityDataStorage), menuName = "Entities/EntityDataStorage")]
    public class EntityDataStorage : ScriptableObject
    {
        [field: SerializeField] public EntityId Id { get; private set; }
        [field: SerializeField] public List<Stat> Stats { get; private set; }
        [field: SerializeField] public BaseEntityBehaviour EntityBehaviourPrefab { get; private set; }
    }
}