using System.Collections.Generic;
using UnityEngine;

namespace NPC.Storages
{
    [CreateAssetMenu(fileName = nameof(EntitiesSpawnerDataStorage), menuName = "Entities/EntitiesSpawnerDataStorage")]
    public class EntitiesSpawnerDataStorage : ScriptableObject
    {
        [field: SerializeField] public List<EntityDataStorage> EntitiesSpawnData { get; private set; }
    }
}