using System;
using UnityEngine;

namespace Core.Services.Updater
{
    public class ProjectUpdater : MonoBehaviour, IProjectUpdater
    {
        public static IProjectUpdater Instance;
        
        private bool _isPaused;
        
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
            }
        }

        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
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
    }
}