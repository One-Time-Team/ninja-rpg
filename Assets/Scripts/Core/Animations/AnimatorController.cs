using System;
using Core.Enums;
using UnityEngine;

namespace Core.Animations
{
    public abstract class AnimatorController : MonoBehaviour
    {
        private AnimationType _currentAnimationType;

        public event Action ActionRequested;
        public event Action ActionEnded;
        
        
        public bool PlayAnimation(AnimationType animationType, bool isActive)
        {
            if (!isActive)
            {
                if (animationType == AnimationType.Idle || animationType != _currentAnimationType)
                    return false;

                _currentAnimationType = AnimationType.Idle;
                PlayAnimation(_currentAnimationType);
                return false;
            }

            if (animationType <= _currentAnimationType)
                return false;

            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
            return true;
        }

        protected abstract void PlayAnimation(AnimationType animationType);

        protected void OnActionRequested() => ActionRequested?.Invoke();
        protected void OnActionEnded() => ActionEnded?.Invoke();
    }
}