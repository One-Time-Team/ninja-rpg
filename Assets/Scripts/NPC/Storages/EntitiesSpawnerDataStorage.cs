using System.Collections.Generic;
using UnityEngine;

namespace NPC.Storages
{
    [CreateAssetMenu(fileName = nameof(EntitiesSpawnerDataStorage), menuName = "EntitiesSystem/EntitiesSpawnerDataStorage")]
    public class EntitiesSpawnerDataStorage : ScriptableObject
    {
        [field: SerializeField] public List<EntityDataStorage> EntitiesSpawnData { get; private set; }
    }
}