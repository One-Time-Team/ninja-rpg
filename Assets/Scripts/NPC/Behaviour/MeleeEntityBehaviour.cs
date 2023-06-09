using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Movement.Controllers;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace NPC.Behaviour
{
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private float _afterAttackDelay;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private Collider2D _hitZone;
        
        [field: SerializeField] public Vector2 SearchBox { get; private set; }
        [field: SerializeField] public LayerMask Target { get; private set; }
        [field: SerializeField] public Slider HPBar { get; private set; }

        public Vector2 Size => _collider2D.bounds.size;

        public event Action AttackSequenceEnded;
        
        
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

        private void EndAttack()
        {
            Animator.ActionEnded -= EndAttack;
            Animator.PlayAnimation(AnimationType.Attack, false);
            Invoke(nameof(EndAttackSequence), _afterAttackDelay);
        }

        private void EndAttackSequence()
        {
            AttackSequenceEnded?.Invoke();
        }

        public void Die()
        {
            Animator.PlayAnimation(AnimationType.Death, true);
            Invoke(nameof(DestroyGameObject), 5);
        }

        private void DestroyGameObject() => Destroy(this.gameObject);
    }
}