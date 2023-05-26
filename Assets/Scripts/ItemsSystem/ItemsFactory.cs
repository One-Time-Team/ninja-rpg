using System;
using ItemsSystem.Core;
using ItemsSystem.Data;
using ItemsSystem.Enums;
using StatsSystem;

namespace Assets.Scripts.ItemsSystem
{
    public class ItemsFactory
    {
        private readonly StatsController _statsController;

        public ItemsFactory(StatsController statsController)
        {
            _statsController = statsController;
        }

        public Item CreateItem(ItemDescriptor itemDescriptor)
        {
            switch (itemDescriptor.ItemType)
            {
                case ItemType.Cloak:
                case ItemType.Gloves:
                case ItemType.Katana:
                    return new Equipment(itemDescriptor, _statsController, GetEquipmentType(itemDescriptor));
                case ItemType.Scroll:
                    return new Scroll(itemDescriptor, _statsController);
                case ItemType.Shuriken:
                    return new Shuriken(itemDescriptor);
                case ItemType.Misc:
                case ItemType.None:
                default:
                    throw new NotImplementedException($"Item with type {itemDescriptor.ItemType} is not implemented yet.");
            }
        }

        private EquipmentType GetEquipmentType(ItemDescriptor itemDescriptor)
        {
            switch (itemDescriptor.ItemType)
            {
                case ItemType.Cloak:
                    return EquipmentType.Cloak;
                case ItemType.Gloves:
                    return EquipmentType.Gloves;
                case ItemType.Katana:
                    return EquipmentType.Scabbard;
                case ItemType.Scroll:
                    return EquipmentType.OnBelt;
                case ItemType.Shuriken:
                    return EquipmentType.OnBelt;
                case ItemType.Misc:
                case ItemType.None:
                default:
                    return EquipmentType.None;
            }
        }
    }
}