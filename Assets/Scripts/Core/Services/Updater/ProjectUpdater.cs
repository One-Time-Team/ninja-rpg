using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Services.Updater
{
    public class ProjectUpdater : MonoBehaviour, IProjectUpdater
    {
        public static IProjectUpdater Instance;
        
        private bool _isPaused;
        private Button[] _buttons;
        private Joystick _joystick;
        
        public event Action UpdateCalled;
        public event Action FixedUpdateCalled;
        public event Action LateUpdateCalled;

        public bool IsPaused
        {
            get => _isPaused;
            
            set
            {
                if (_isPaused == value)
                    return;

                Time.timeScale = value ? 0 : 1;
                _isPaused = value;

                _joystick.enabled = !_joystick.enabled;
                foreach (Button button in _buttons)
                {
                    button.interactable = !button.interactable;
                }
            }
        }

        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            _buttons = FindObjectsOfType<Button>();
            _joystick = FindObjectOfType<FixedJoystick>();
        }

        private void Update()
        {
            if (IsPaused)
                return;
            
            UpdateCalled?.Invoke();
        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;
            
            FixedUpdateCalled?.Invoke();
        }

        private void LateUpdate()
        {
            if (IsPaused)
                return;
            
            LateUpdateCalled?.Invoke();
        }
    }
}