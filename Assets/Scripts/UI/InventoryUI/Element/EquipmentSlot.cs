using System.Reflection;
using ItemsSystem.Core;
using ItemsSystem.Enums;
using UnityEngine;

namespace UI.InventoryUI.Element
{
    public class EquipmentSlot : ItemSlot
    {
        [field: SerializeField] public EquipmentType EquipmentType { get; private set; }

        public void SetAfterImage(Sprite sprite, Sprite backSprite)
        {
            SetItem(sprite, backSprite, -1);
            RemoveButton.gameObject.SetActive(false);
        }
    }
}