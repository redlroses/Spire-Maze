using UnityEngine.Events;

namespace CodeBase.UI.Elements.Buttons
{
    public class SimpleButton : ButtonObserver
    {
        public event UnityAction Clicked
        {
            add => Button.onClick.AddListener(value);
            remove => Button.onClick.RemoveListener(value);
        }
    }
}