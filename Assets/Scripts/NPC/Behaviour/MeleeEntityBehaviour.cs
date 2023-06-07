using System;
using Core.Enums;
using Core.Movement.Controllers;
using UnityEngine;

namespace NPC.Behaviour
{
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private float _afterAttackDelay;
        [SerializeField] private Collider2D _collider2D;
        
        [field: SerializeField] public Vector2 SearchBox { get; private set; }
        [field: SerializeField] public LayerMask Target { get; private set; }

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

            Animator.ActionRequested += Attack;
            Animator.ActionEnded += EndAttack;
        }
        
        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void EndAttack()
        {
            Animator.ActionRequested -= Attack;
            Animator.ActionEnded -= EndAttack;
            Animator.PlayAnimation(AnimationType.Attack, false);
            Invoke(nameof(EndAttackSequence), _afterAttackDelay);
        }

        private void EndAttackSequence()
        {
            AttackSequenceEnded?.Invoke();
        }
    }
}