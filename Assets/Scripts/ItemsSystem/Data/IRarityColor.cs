using ItemsSystem.Enums;
using UnityEngine;

namespace ItemsSystem.Data
{
    public interface IRarityColor
    {
        ItemRarity Rarity { get; }
        Color Color { get; }
    }
}