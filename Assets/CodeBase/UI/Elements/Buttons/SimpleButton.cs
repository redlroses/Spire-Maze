using System;

namespace CodeBase.UI.Elements.Buttons
{
    internal class SimpleButton : ButtonObserver
    {
        private Action _action;

        protected override void Call() =>
            _action?.Invoke();

        public void SetAction(Action action) =>
            _action = action;
    }
}