using Core.Animations;
using Core.Enums;
using Core.Movement.Controllers;
using System;
using UnityEngine;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseEntityBehaviour : MonoBehaviour
    {
        [SerializeField] public AnimatorController Animator;
        
        protected Rigidbody2D Rigidbody;
        protected Mover Mover;
        
        public event Action<float> DamageTaken;

        public void TakeDamage(float damage) => DamageTaken?.Invoke(damage);

        public virtual void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody2D>();    
        }

        public void MoveHorizontally(float direction) => Mover.MoveHorizontally(direction);
        public void MoveVertically(float direction) => Mover.MoveVertically(direction);
        
        protected virtual void UpdateAnimations()
        {
            Animator.PlayAnimation(AnimationType.Run, Mover.IsMoving);
            Animator.PlayAnimation(AnimationType.Idle, true);
        }
    }
}