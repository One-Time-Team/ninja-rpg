using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _attackButton;

        [field: SerializeField] public Button InteractButton { get; private set; }

        public float HorizontalDirection => _joystick.Horizontal;
        public bool Jumped { get; private set; }
        public bool AttackStarted { get; private set; }


        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => Jumped = true);
            _attackButton.onClick.AddListener(() => AttackStarted = true);
        }

        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
            _attackButton.onClick.RemoveAllListeners();
        }

        public void ResetOneTimeActions()
        {
            Jumped = false;
            AttackStarted = false;
        }
    }
}