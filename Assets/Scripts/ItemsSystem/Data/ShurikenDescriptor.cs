using System;
using ItemsSystem.Enums;
using UnityEngine;

namespace ItemsSystem.Data
{
    [Serializable]
    public class ShurikenDescriptor : ItemDescriptor
    {
        [field: SerializeField] public ShurikenType ShurikenType { get; private set; }

        public ShurikenDescriptor(ItemId id, ItemType itemType, Sprite sprite, ItemRarity rarity, float price,
            ShurikenType shurikenType) : base(id, itemType, sprite, rarity, price)
        {
            ShurikenType = shurikenType;
        }
    }
}