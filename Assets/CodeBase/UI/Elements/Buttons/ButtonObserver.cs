using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonObserver : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Awake() =>
            _button ??= GetComponent<Button>();

        public void Subscribe() =>
            _button.onClick.AddListener(Call);

        public void Subscribe(UnityAction call) =>
            _button.onClick.AddListener(call);

        public void Cleanup() =>
            _button.onClick.RemoveAllListeners();

        protected abstract void Call();
    }
}