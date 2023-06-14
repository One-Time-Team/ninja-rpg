using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _attackButton;
        
        private List<Button> _buttons;

        [field: SerializeField] public Joystick Joystick { get; private set; }
        [field: SerializeField] public Button InteractButton { get; private set; }
        
        public List<Button> Buttons
        {
            get
            {
                if (_buttons == null)
                {
                    _buttons = new List<Button>()
                    {
                        _jumpButton,
                        _attackButton,
                        InteractButton,
                    };
                }

                return _buttons;
            }
        }

        public float HorizontalDirection => Joystick.Horizontal;
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