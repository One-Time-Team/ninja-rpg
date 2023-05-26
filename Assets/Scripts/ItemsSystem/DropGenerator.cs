using Core.Services.Updater;
using ItemsSystem.Data;
using ItemsSystem.Enums;
using Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.ItemsSystem
{
    class DropGenerator
    {
        private PlayerEntityHandler _playerEntityHandler;
        private List<ItemDescriptor> _itemDescriptors;
        private ItemSystem _itemsSystem;

        public DropGenerator(PlayerEntityHandler playerEntityHandler, List<ItemDescriptor> itemDescriptors, ItemSystem itemsSystem)
        {
            _playerEntityHandler = playerEntityHandler;
            _itemDescriptors = itemDescriptors;
            _itemsSystem = itemsSystem;
            ProjectUpdater.Instance.UpdateCalled += Update;
        }

        private void DropRandomItem(ItemRarity itemRarity)
        {
            List<ItemDescriptor> items = _itemDescriptors.Where(item => item.Rarity == itemRarity).ToList();
            ItemDescriptor itemDescriptor = items[Random.Range(0, items.Count())];
            _itemsSystem.DropItem(itemDescriptor, _playerEntityHandler.transform.position);
        }

        private ItemRarity GetDropRarity()
        {
            return ItemRarity.Trash;           
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
                DropRandomItem(GetDropRarity());
        }
    }
}
