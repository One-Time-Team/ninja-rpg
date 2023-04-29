using System;
using Core.Enums;
using UnityEngine;

namespace StatsSystem
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

        public static implicit operator float(Stat stat) => stat?.Value ?? 0;

        public void SetValue(float value) => Value = value;

        public Stat GetCopy() => new Stat(Type, Value);
    }
}