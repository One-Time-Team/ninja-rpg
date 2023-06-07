using System;
using ItemsSystem.Enums;
using UnityEngine;

namespace ItemsSystem.Data
{
    [Serializable]
    public class ItemDescriptor
    {
        [field: SerializeField] public ItemId Id { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ItemRarity Rarity { get; private set; }
        [field: SerializeField] public float Price { get; private set; }

        public ItemDescriptor(ItemId id, ItemType itemType, Sprite sprite, ItemRarity rarity, float price)
        {
            Id = id;
            ItemType = itemType;
            Sprite = sprite;
            Rarity = rarity;
            Price = price;
        }
    }
}