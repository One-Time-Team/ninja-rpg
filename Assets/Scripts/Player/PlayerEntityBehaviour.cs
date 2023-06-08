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
        
        private Jumper _jumper;


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

            Animator.ActionRequested += Attack;
            Animator.ActionEnded += EndAttack;
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
        
        private void Attack()
        {
            Debug.Log("Attack has been committed");
        }

        private void EndAttack()
        {
            Animator.ActionRequested -= Attack;
            Animator.ActionEnded -= EndAttack;
            Animator.PlayAnimation(AnimationType.Attack, false);
        }
    }
}
