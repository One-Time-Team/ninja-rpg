using System;
using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource, IWindowsInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _inventoryButton;
        [field: SerializeField] public Button InteractButton { get; private set; }

        public float HorizontalDirection => _joystick.Horizontal;
        public bool IsJumping { get; private set; }
        public bool IsAttacking { get; private set; }

        public event Action InventoryRequested;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => IsJumping = true);
            _attackButton.onClick.AddListener(() => IsAttacking = true);
        }

        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
            _attackButton.onClick.RemoveAllListeners();
        }

        public void ResetOneTimeActions()
        {
            IsJumping = false;
            IsAttacking = false;
        }
    }
}