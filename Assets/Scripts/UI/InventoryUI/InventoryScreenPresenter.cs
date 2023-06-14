using System.Collections.Generic;
using System.Linq;
using ItemsSystem;
using ItemsSystem.Core;
using ItemsSystem.Data;
using ItemsSystem.Enums;
using UI.Core;
using UI.InventoryUI.Element;
using UnityEngine;

namespace UI.InventoryUI
{
    public class InventoryScreenPresenter : ScreenController<InventoryScreenView>
    {
        private readonly Inventory _inventory;
        private readonly List<RarityDescriptor> _rarityDescriptors;

        private readonly Dictionary<ItemSlot, Item> _backPackSlots;
        private readonly Dictionary<EquipmentSlot, Equipment> _equipmentSlots;

        private readonly EquipmentConditionChecker _equipmentConditionChecker;

        private readonly Sprite _emptyBackSprite;

        public InventoryScreenPresenter(InventoryScreenView view, Inventory inventory,
            List<RarityDescriptor> rarityDescriptors) : base(view)
        {
            _inventory = inventory;

            _rarityDescriptors = rarityDescriptors;
            _emptyBackSprite = _rarityDescriptors.Find(descriptor => descriptor.Rarity == ItemRarity.None).Sprite;
            _backPackSlots = new Dictionary<ItemSlot, Item>();
            _equipmentSlots = new Dictionary<EquipmentSlot, Equipment>();
            _equipmentConditionChecker = new EquipmentConditionChecker();
        }

        public override void Initialize()
        {
            View.MovingImage.gameObject.SetActive(false);
            InitializeBackPack();
            InitializeEquipment();
            _inventory.BackPackChanged += UpdateBackPack;
            _inventory.EquipmentChanged += UpdateEquipment;
            base.Initialize();
        }

        public override void Complete()
        {
            View.Hide();
            ClearBackPack();
            ClearEquipment();
            _inventory.BackPackChanged -= UpdateBackPack;
            _inventory.EquipmentChanged -= UpdateEquipment;
        }

        private void InitializeBackPack()
        {
            var backPack = View.ItemSlots;
            for (var i = 0; i < backPack.Count; i++)
            {
                var slot = backPack[i];
                var item = _inventory.BackPackItems[i];
                _backPackSlots.Add(slot, item);

                if (item == null)
                    continue;

                slot.SetItem(item.ItemDescriptor.Sprite, GetBackSprite(item.ItemDescriptor.Rarity), item.Amount);
                SubscribeToSlotEvents(slot);
            }
        }

        private void InitializeEquipment()
        {
            var equipment = View.EquipmentSlots;
            for (var i = 0; i < equipment.Count; i++)
            {
                var slot = equipment[i];
                var item = _inventory.EquipmentItems[i];
                _equipmentSlots.Add(slot, item);

                if (item == null)
                    continue;

                slot.SetItem(item.ItemDescriptor.Sprite, GetBackSprite(item.ItemDescriptor.Rarity), item.Amount);
                SubscribeToSlotEvents(slot);
            }
        }

        private void SubscribeToSlotEvents(ItemSlot slot)
        {
            slot.SlotClicked += UseSlot;
            slot.SlotClearClicked += ClearSlot;
        }

        private void UnsubscribeFromSlotEvents(ItemSlot slot)
        {
            slot.SlotClicked -= UseSlot;
            slot.SlotClearClicked -= ClearSlot;
        }

        private Sprite GetBackSprite(ItemRarity rarity) =>
            _rarityDescriptors.Find(descriptor => descriptor.Rarity == rarity).Sprite;

        private void UpdateBackPack()
        {
            ClearBackPack();
            InitializeBackPack();
        }

        private void UpdateEquipment()
        {
            ClearEquipment();
            InitializeEquipment();
        }

        private void UseSlot(ItemSlot slot)
        {
            Equipment equipment;
            if (slot is EquipmentSlot equipmentSlot && _inventory.BackPackItems.Any(item => item == null))
            {
                equipment = _equipmentSlots[equipmentSlot];
                _inventory.UnEquip(equipment, false);
                _inventory.AddBackPackItem(equipment);
                equipment?.Use();
                return;
            }

            Item item = _backPackSlots[slot];

            if (item is Scroll scroll)
            {
                scroll.Use();
                if (scroll.Amount <= 0)
                    _inventory.RemoveBackPackItem(scroll, false);

                return;
            }

            if (item is not Equipment equip)
                return;

            equipment = equip;

            if (!_equipmentConditionChecker.IsEquipmentConditionFits(equipment, _inventory.EquipmentItems)
                || !_equipmentConditionChecker.TryReplaceEquipment(equipment, _inventory.EquipmentItems,
                    out var prevEquipment))
                return;

            _inventory.RemoveBackPackItem(equipment, false);
            if (prevEquipment != null)
            {
                _inventory.AddBackPackItem(prevEquipment);
                prevEquipment.Use();
            }

            _inventory.Equip(equipment);
            equipment.Use();
        }

        private void ClearSlot(ItemSlot slot)
        {
            if (_backPackSlots.TryGetValue(slot, out Item item))
                _inventory.RemoveBackPackItem(item, true);

            if (slot is EquipmentSlot equipmentSlot &&
                _equipmentSlots.TryGetValue(equipmentSlot, out Equipment equipment))
                _inventory.UnEquip(equipment, true);
        }

        private void ClearSlots(List<ItemSlot> slots)
        {
            foreach (var slot in slots)
            {
                UnsubscribeFromSlotEvents(slot);
                slot.ClearItem(_emptyBackSprite);
            }
        }

        private void ClearBackPack()
        {
            ClearSlots(_backPackSlots.Select(item => item.Key).ToList());
            _backPackSlots.Clear();
        }

        private void ClearEquipment()
        {
            ClearSlots(_equipmentSlots.Select(item => item.Key).Cast<ItemSlot>().ToList());
            _equipmentSlots.Clear();
        }
    }
}