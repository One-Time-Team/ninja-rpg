using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using ItemsSystem.Data;
using ItemsSystem.Enums;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ItemsSystem
{
    class DropGenerator : IDisposable
    {
        private readonly PlayerEntityHandler _playerEntityHandler;
        private readonly List<ItemDescriptor> _itemDescriptors;
        private readonly ItemSystem _itemsSystem;

        public DropGenerator(PlayerEntityHandler playerEntityHandler, List<ItemDescriptor> itemDescriptors, ItemSystem itemsSystem)
        {
            _playerEntityHandler = playerEntityHandler;
            _itemDescriptors = itemDescriptors;
            _itemsSystem = itemsSystem;
            ProjectUpdater.Instance.UpdateCalled += Update;
        }

        public void Dispose()
        {
            ProjectUpdater.Instance.UpdateCalled -= Update;
        }

        private void DropRandomItem(ItemRarity itemRarity)
        {
            List<ItemDescriptor> items = _itemDescriptors.Where(item => item.Rarity == itemRarity).ToList();
            ItemDescriptor itemDescriptor = items[Random.Range(0, items.Count())];
            _itemsSystem.DropItem(itemDescriptor, _playerEntityHandler.transform.position);
        }

        private ItemRarity GetDropRarity()
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

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
                DropRandomItem(GetDropRarity());
        }
    }
}
