using Core.Animations;
using Core.Enums;
using Core.Movement.Controllers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseEntityBehaviour : MonoBehaviour
    {
        [SerializeField] protected AnimatorController Animator;
        
        protected Rigidbody2D Rigidbody;
        protected Mover Mover;

        [field: SerializeField] public Slider HPBar { get; private set; }
        
        public event Action<float> DamageTaken;

        
        public virtual void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody2D>();    
        }

        public void TakeDamage(float damage) => DamageTaken?.Invoke(damage);
        
        public void MoveHorizontally(float direction) => Mover.MoveHorizontally(direction);

        public virtual void Die() => Animator.PlayAnimation(AnimationType.Death, true);
        
        protected virtual void UpdateAnimations()
        {
            Animator.PlayAnimation(AnimationType.Run, Mover.IsMoving);
            Animator.PlayAnimation(AnimationType.Idle, true);
        }
    }
}