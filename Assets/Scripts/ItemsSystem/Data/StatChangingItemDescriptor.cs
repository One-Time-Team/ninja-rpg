using System;
using System.Collections.Generic;
using ItemsSystem.Enums;
using StatsSystem.Data;
using UnityEngine;

namespace ItemsSystem.Data
{
    [Serializable]
    public class StatChangingItemDescriptor : ItemDescriptor
    {
        [field: SerializeField] public float StatChangingLevel { get; private set; }
        [field: SerializeField] public List<StatModificator> Stats { get; private set; }

        public StatChangingItemDescriptor(ItemId id, ItemType itemType, Sprite sprite, ItemRarity rarity, float price,
            float statChangingLevel, List<StatModificator> stats) : base(id, itemType, sprite, rarity, price)
        {
            StatChangingLevel = statChangingLevel;
            Stats = stats;
        }
    }
}