using System;
using Core.Enums;
using UnityEngine;

namespace Core.Animations
{
    public abstract class AnimatorController : MonoBehaviour
    {
        public AnimationType CurrentAnimationType { get; private set; }
        
        public event Action ActionRequested;
        public event Action ActionEnded;
        
        
        public bool PlayAnimation(AnimationType animationType, bool isActive)
        {
            if (!isActive)
            {
                if (animationType == AnimationType.Idle || animationType != CurrentAnimationType)
                    return false;

                CurrentAnimationType = AnimationType.Idle;
                PlayAnimation(CurrentAnimationType);
                return false;
            }

            if (animationType <= CurrentAnimationType)
                return false;

            CurrentAnimationType = animationType;
            PlayAnimation(CurrentAnimationType);
            return true;
        }

        protected abstract void PlayAnimation(AnimationType animationType);

        protected void OnActionRequested() => ActionRequested?.Invoke();
        protected void OnActionEnded() => ActionEnded?.Invoke();
    }
}