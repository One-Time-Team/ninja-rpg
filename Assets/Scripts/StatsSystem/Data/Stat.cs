using System;
using StatsSystem.Enums;
using UnityEngine;

namespace StatsSystem.Data
{
    [Serializable]
    public class Stat
    {
        [field: SerializeField] public StatType Type { get; private set; }
        [field: SerializeField] public float Value { get; private set; }


        public Stat(StatType type, float value)
        {
            Type = type;
            Value = value;
        }

        public static implicit operator float(Stat stat)
        {
            if (stat == null) return Mathf.Epsilon;
            
            return stat.Value == 0 ? Mathf.Epsilon : stat.Value;
        }

        public void SetValue(float value) => Value = value;

        public Stat GetCopy() => new Stat(Type, Value);
    }
}