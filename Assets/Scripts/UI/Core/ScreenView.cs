using UnityEngine;

namespace UI.Core
{
    public abstract class ScreenView : MonoBehaviour
    {
        [SerializeField] private Canvas _root;

        public virtual void Show() => _root.enabled = true;
        public virtual void Hide() => _root.enabled = false;
    }
}