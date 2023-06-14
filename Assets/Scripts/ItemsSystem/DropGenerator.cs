using System.Collections.Generic;
using System.Linq;
using ItemsSystem.Data;
using ItemsSystem.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ItemsSystem
{
    public class DropGenerator
    {
        private readonly List<ItemDescriptor> _itemDescriptors;
        private readonly ItemSystem _itemsSystem;

        public DropGenerator(List<ItemDescriptor> itemDescriptors, ItemSystem itemsSystem)
        {
            _itemDescriptors = itemDescriptors;
            _itemsSystem = itemsSystem;
        }

        public void DropRandomItem(Vector2 position)
        {
            ItemRarity itemRarity = GetRandomDropRarity();
            List<ItemDescriptor> items = _itemDescriptors.Where(item => item.Rarity == itemRarity).ToList();
            ItemDescriptor itemDescriptor = items[Random.Range(0, items.Count())];
            _itemsSystem.DropItem(itemDescriptor, position);
        }

        private ItemRarity GetRandomDropRarity()
        {
            float chance = Random.Range(0, 100);
            return chance switch
            {
                <= 40 => ItemRarity.Trash,
                > 40 and <= 70 => ItemRarity.Common,
                > 70 and <= 90 => ItemRarity.Rare,
                > 90 and <= 97 => ItemRarity.Epic,
                > 97 and <= 100 => ItemRarity.Legendary,
                _ => ItemRarity.Trash
            };
        }
    }
}
