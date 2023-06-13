using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Movement.Controllers;
using Pathfinding;
using UnityEngine;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Seeker))]
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        private const float LandingDetectionTime = 0.4f;
        
        [SerializeField] private float _afterAttackDelay;
        [SerializeField] private Collider2D _entityCollider;
        [SerializeField] private Collider2D _hitZone;
        [SerializeField] private float _corpseDisappearingTime;

        private Jumper _jumper;

        [field: SerializeField] public Vector2 TargetSearchBox { get; private set; }
        [field: SerializeField] public LayerMask Target { get; private set; }
        [field: SerializeField] public LayerMask Ground { get; private set; }
        [field: SerializeField] public bool FollowEnabled { get; private set; } = true;
        [field: SerializeField] public bool JumpEnabled { get; private set; } = true;

        public Vector2 Size => _entityCollider.bounds.size;
        public Vector2 Center => _entityCollider.bounds.center;
        public Vector2 Position => Rigidbody.position;

        public event Action AttackImpacted;
        public event Action AttackSequenceEnded;
        public event Action Disappeared;
        
        
        private void Update() => UpdateAnimations();
        
        private void OnCollisionStay2D(Collision2D other)
        {
            if (_jumper.GetOnGround(other))
                CancelInvoke(nameof(StartLanding));
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (_jumper.GetOffGround(other))
                Invoke(nameof(StartLanding), LandingDetectionTime);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, TargetSearchBox);
        }
        
        public override void Initialize()
        {
            base.Initialize();
            Mover = new VelocityMover(Rigidbody);
            _jumper = new Jumper(Rigidbody);
        }

        public void Jump(float jumpForce)
        {
            if (!_jumper.StartJump(jumpForce))
                return;
            
            if (!Animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping))
                return;
            
            Animator.ActionEnded += EndJump;
        }

        public void Land()
        {
            _jumper.Land();
            Animator.PlayAnimation(AnimationType.Land, false);
            Animator.PlayAnimation(AnimationType.Jump, false);
        }

        public void StartAttack()
        {
            if (!Animator.PlayAnimation(AnimationType.Attack, true))
                return;
            
            _hitZone.enabled = true;

            Animator.ActionRequested += OnAttackImpacted;
            Animator.ActionEnded += EndAttack;
        }
        
        public bool TryGetAttackTarget(out BaseEntityBehaviour target)
        {
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D
            {
                useLayerMask = true
            };
            filter.SetLayerMask(Target);

            target = null;
            var numOfTargets = _hitZone.OverlapCollider(filter, results);
            return numOfTargets != 0 && results[0].TryGetComponent(out target);
        }
        
        public override void Die()
        {
            base.Die();
            Invoke(nameof(DestroyGameObject), _corpseDisappearingTime);
        }

        public void SetDirection(Direction direction) => Mover.SetDirection(direction);
        
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
            Invoke(nameof(OnAttackSequenceEnded), _afterAttackDelay);
        }

        private void OnAttackSequenceEnded()
        {
            AttackSequenceEnded?.Invoke();
        }

        private void DestroyGameObject()
        {
            Disappeared?.Invoke();
            Destroy(gameObject);
        }
    }
}