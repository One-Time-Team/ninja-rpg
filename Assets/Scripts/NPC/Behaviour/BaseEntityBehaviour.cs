using Core.Animations;
using Core.Enums;
using Core.Movement.Controllers;
using UnityEngine;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseEntityBehaviour : MonoBehaviour
    {
        [SerializeField] protected AnimatorController Animator;
        
        protected Rigidbody2D Rigidbody;
        protected Mover Mover;

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