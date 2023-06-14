using System;
using UI.Enum;

namespace UI.Core
{
    public abstract class ScreenController<TView> : IScreenController where TView : ScreenView
    {
        protected TView View;
        public ScreenController(TView view) => View = view;

        public event Action CloseRequested;
        public event Action<ScreenType> OpenScreenRequested;
        public virtual void Initialize() => View.Show();
        public virtual void Complete() => View.Hide();

        protected void RequestClose() => CloseRequested?.Invoke();

        protected void RequestScreen(ScreenType characterScreenType) =>
            OpenScreenRequested?.Invoke(characterScreenType);
    }
}