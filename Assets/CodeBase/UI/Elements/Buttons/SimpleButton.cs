using UnityEngine.Events;

namespace CodeBase.UI.Elements.Buttons
{
    public class SimpleButton : ButtonObserver
    {
        public void Subscribe(UnityAction call) =>
            Button.onClick.AddListener(call);
    }
}