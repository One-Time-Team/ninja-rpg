using System.Collections.Generic;
using StatsSystem.Data;
using UnityEngine;

namespace StatsSystem.Storages
{
    [CreateAssetMenu(fileName = nameof(StatsStorage), menuName = "StatsSystem/StatsStorage")]
    public class StatsStorage : ScriptableObject
    {
        [field: SerializeField] public List<Stat> Stats { get; private set; }
    }
}