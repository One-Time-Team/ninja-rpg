using System;
using ItemsSystem.Enums;
using UnityEngine;

namespace ItemsSystem.Data
{
    [Serializable]
    public class RarityDescriptor : IRarityColor
    {
        [field: SerializeField] public ItemRarity Rarity { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}