using System;
using StatsSystem.Enums;
using UnityEngine;

namespace StatsSystem.Data
{
    [Serializable]
    public class StatModificator
    {
        [field: SerializeField] public Stat Stat { get; private set; }
        [field: SerializeField] public StatModificatorType Type { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }

        public float StartTime { get; }


        public StatModificator(Stat stat, StatModificatorType type, float duration, float startTime)
        {
            Stat = stat;
            Type = type;
            Duration = duration;
            StartTime = startTime;
        }

        public StatModificator GetReversed()
        {
            var reversedStat = new Stat(Stat.Type, Type == StatModificatorType.Additive 
                ? -Stat 
                : 1 / Stat);
            return new StatModificator(reversedStat, Type, Duration, Time.time);
        }
    }
}