using System;
using Core.Services.Updater;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputReader
{
    public class ExternalDeviceInputReader : IEntityInputSource, IDisposable
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public bool Jumped { get; private set; }
        public bool AttackStarted { get; private set; }


        public ExternalDeviceInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }
        
        public void ResetOneTimeActions()
        {
            Jumped = false;
            AttackStarted = false;
        }
        
        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jumped = true;
            }

            if (!IsPointerOverUI() && Input.GetButtonDown("Fire1"))
            {
                AttackStarted = true;
            }
        }
        
        private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    }
}
