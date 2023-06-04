using Core.Enums;
using UnityEngine;

namespace Core.Animations
{
    [RequireComponent(typeof(Animator))]
    public class UnityAnimatorController : AnimatorController
    {
        private Animator _animator;
        
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
        
        protected override void PlayAnimation(AnimationType animationType)
        {
            _animator.SetInteger(nameof(AnimationType), (int)animationType);
        }
    }
}