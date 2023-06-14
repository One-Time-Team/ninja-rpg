using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryUI.Element
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private Image _itemBack;
        [SerializeField] private Image _emptyImage;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _itemAmount;

        [SerializeField] protected Button RemoveButton;
        [SerializeField] private Button _slotButton;

        public event Action<ItemSlot> SlotClearClicked;
        public event Action<ItemSlot> SlotClicked;

        private void Awake()
        {
            RemoveButton.onClick.AddListener(() => SlotClearClicked?.Invoke(this));
            _slotButton.onClick.AddListener(() => SlotClicked?.Invoke(this));
        }

        public void SetItem(Sprite iconSprite, Sprite itemBackSprite, int amount)
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = iconSprite;
            _emptyImage.gameObject.SetActive(false);
            _itemBack.sprite = itemBackSprite;
            RemoveButton.gameObject.SetActive(true);

            if (_itemAmount == null)
                return;
            
            _itemAmount.gameObject.SetActive(amount > 0);
            _itemAmount.text = amount.ToString();
        }

        public void ClearItem(Sprite emptyBackSprite)
        {
            _itemBack.sprite = emptyBackSprite;
            _icon.gameObject.SetActive(false);
            _emptyImage.gameObject.SetActive(true);
            RemoveButton.gameObject.SetActive(false);
            if (_itemAmount != null)
                _itemAmount.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            RemoveButton.onClick.RemoveAllListeners();
            _slotButton.onClick.RemoveAllListeners();
        }
    }
}