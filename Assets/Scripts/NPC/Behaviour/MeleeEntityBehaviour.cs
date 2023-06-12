using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Movement.Controllers;
using UnityEngine;

namespace NPC.Behaviour
{
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private float _afterAttackDelay;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private Collider2D _hitZone;
        [SerializeField] private float _disappearingTime;

        [field: SerializeField] public Vector2 SearchBox { get; private set; }
        [field: SerializeField] public LayerMask Target { get; private set; }

        public Vector2 Size => _collider2D.bounds.size;

        public event Action AttackImpacted;
        public event Action AttackSequenceEnded;
        public event Action Disappeared;
        
        
        private void Update() => UpdateAnimations();
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, SearchBox);
        }
        
        public override void Initialize()
        {
            base.Initialize();
            Mover = new PositionMover(Rigidbody);
        }

        public void SetDirection(Direction direction) => Mover.SetDirection(direction);

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
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.SetLayerMask(Target);

            target = null;
            var numOfTargets = _hitZone.OverlapCollider(filter, results);
            return numOfTargets != 0 && results[0].TryGetComponent(out target);
        }

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

        public override void Die()
        {
            base.Die();
            Invoke(nameof(DestroyGameObject), _disappearingTime);
        }

        private void DestroyGameObject()
        {
            Disappeared?.Invoke();
            Destroy(gameObject);
        }
    }
}