using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Movement.Controllers;
using Core.Tools;
using NPC.Behaviour;
using UnityEngine;

namespace Player{
    
    public class PlayerEntityBehaviour : BaseEntityBehaviour
    {
        private const float LandingDetectionTime = 0.4f;
        
        [SerializeField] private Cameras _cameras;
        [SerializeField] private Collider2D _hitZone;
        
        private Jumper _jumper;
        
        [field: SerializeField] public LayerMask Target { get; private set; }
        
        public bool IsAttackProcessing { get; private set; }
        
        public event Action AttackImpacted;


        private void Update()
        {
            UpdateAnimations();
            UpdateCameras();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_jumper.GetOnGround(other))
                CancelInvoke(nameof(StartLanding));
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (_jumper.GetOffGround(other))
                Invoke(nameof(StartLanding), LandingDetectionTime);
        }
        
        public override void Initialize() {
            base.Initialize();
            Mover = new VelocityMover(Rigidbody);
            _jumper = new Jumper(Rigidbody);
            _hitZone.enabled = false;
        }
        
        public void UpdateCameras()
        {
            if (_cameras.StartCamera.enabled || _cameras.FinalCamera.enabled) return;

            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = Mover.FaceDirection == cameraPair.Key;
        }

        public void Jump(float jumpForce)
        {
            if (!_jumper.StartJump(jumpForce))
                return;
            
            if (!Animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping))
                return;
            
            Animator.ActionEnded += EndJump;
        }

        public void StartAttack()
        {
            if (!Animator.PlayAnimation(AnimationType.Attack, true))
                return;
            
            _hitZone.enabled = true;
            IsAttackProcessing = true;

            Animator.ActionRequested += OnAttackImpacted;
            Animator.ActionEnded += EndAttack;
        }
        
        public bool TryGetAttackTarget(out BaseEntityBehaviour target)
        {
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.SetLayerMask(Target);

            target = null;
            var numOfTargets = _hitZone.OverlapCollider(filter, results);
            return numOfTargets != 0 && results[0].TryGetComponent(out target);
        }

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            Animator.PlayAnimation(AnimationType.Land, _jumper.IsLanding);
        }

        private void EndJump()
        {
            Animator.ActionEnded -= EndJump;
            StartLanding();
        }
        
        private void StartLanding() => _jumper.StartLanding();
        
        private void OnAttackImpacted()
        {
            AttackImpacted?.Invoke();
        }

        private void EndAttack()
        {
            Animator.ActionRequested -= OnAttackImpacted;
            Animator.ActionEnded -= EndAttack;
            Animator.PlayAnimation(AnimationType.Attack, false);
            _hitZone.enabled = false;
            IsAttackProcessing = false;
        }
    }
}
