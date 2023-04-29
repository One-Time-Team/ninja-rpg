using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _attackButton;

        public float HorizontalDirection => _joystick.Horizontal;
        public bool IsJumping { get; private set; }
        public bool IsAttacking { get; private set; }


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