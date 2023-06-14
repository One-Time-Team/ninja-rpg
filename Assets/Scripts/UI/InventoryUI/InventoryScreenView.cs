using System;
using System.Collections.Generic;
using System.Linq;
using ItemsSystem.Core;
using TMPro;
using UI.Core;
using UI.InventoryUI.Element;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public class InventoryScreenView : ScreenView
    {
        [SerializeField] private Button _removeButton;
        [SerializeField] private TMP_Text _goldText;

        [SerializeField] private Transform _backPackContainer;
        [SerializeField] private Transform _equipmentContainer;
        
        public List<ItemSlot> ItemSlots { get; private set; } 
        public List<EquipmentSlot> EquipmentSlots { get; private set; } 
        
        [field: SerializeField] public Image MovingImage { get; private set; }

        public event Action CloseClicked;

        private void Awake()
        {
            _removeButton.onClick.AddListener(() => CloseClicked?.Invoke());
            ItemSlots = GetComponentsInChildren<ItemSlot>().ToList();
            EquipmentSlots = GetComponentsInChildren<EquipmentSlot>().ToList();
        }

        private void OnDestroy()
        {
            _removeButton.onClick.RemoveAllListeners();
        }

        public void SetGoldAmount(string amount) => _goldText.text = amount;
    }
}