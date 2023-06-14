using System;
using System.Collections.Generic;
using ItemsSystem.Core;

namespace ItemsSystem
{
    public class Inventory
    {
        public const int InventorySize = 28;
        public List<Item> BackPackItems { get; }
        public List<Equipment> EquipmentItems { get; }

        public event Action BackPackChanged;
        public event Action EquipmentChanged;

        public Inventory(List<Item> backPackItems, List<Equipment> equipmentItems)
        {
            EquipmentItems = equipmentItems ?? new List<Equipment>();
            if (backPackItems != null)
                return;

            BackPackItems = new List<Item>();
            for (var i = 0; i < InventorySize; i++)
                BackPackItems.Add(null);
        }

        public void AddBackPackItem(Item item)
        {
            var index = BackPackItems.FindIndex(element => element == null);
            BackPackItems[index] = item;
            BackPackChanged?.Invoke();
        }

        public void RemoveBackPackItem(Item item, bool toWorld)
        {
            var index = BackPackItems.IndexOf(item);
            BackPackItems[index] = null;
            BackPackChanged?.Invoke();
            
        }

        public void Equip(Equipment equipment)
        {
            EquipmentItems.Add(equipment);
            EquipmentChanged?.Invoke();
        }

        public void UnEquip(Equipment equipment, bool toWorld)
        {
            EquipmentItems.Remove(equipment);
            EquipmentChanged?.Invoke();
            
        }
    }
}