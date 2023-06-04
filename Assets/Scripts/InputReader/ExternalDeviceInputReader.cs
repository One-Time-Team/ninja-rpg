using System;
using Core.Services.Updater;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputReader
{
    public class ExternalDeviceInputReader : IEntityInputSource, IDisposable
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public bool IsJumping { get; private set; }
        public bool IsAttacking { get; private set; }


        public ExternalDeviceInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }
        
        public void ResetOneTimeActions()
        {
            IsJumping = false;
            IsAttacking = false;
        }
        
        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
            {
                IsJumping = true;
            }

            if (!IsPointerOverUI() && Input.GetButtonDown("Fire1"))
            {
                IsAttacking = true;
            }
        }
        
        private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    }
}
