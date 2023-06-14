using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Services.Updater
{
    public class ProjectUpdater : MonoBehaviour, IProjectUpdater
    {
        public static IProjectUpdater Instance;
        
        private bool _isPaused;
        private List<Button> _buttons;
        private Joystick _joystick;
        
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
        
        public event Action UpdateCalled;
        public event Action FixedUpdateCalled;
        public event Action LateUpdateCalled;

        Coroutine IProjectUpdater.StartCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
        void IProjectUpdater.StopCoroutine(Coroutine coroutine) => StopCoroutine(coroutine);
        void IProjectUpdater.StopAllCoroutines() => StopAllCoroutines();
        

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
                
            else
                Destroy(gameObject);
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

        private void OnDestroy()
        {
            Instance.StopAllCoroutines();
        }

        public void Initialize(Joystick joystick, List<Button> buttons)
        {
            _joystick = joystick;
            _buttons = buttons;
        }
    }
}