using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonObserver : MonoBehaviour
    {
        [FormerlySerializedAs("_button")] [SerializeField] protected Button Button;

        public event UnityAction Clicked
        {
            add => Button.onClick.AddListener(value);
            remove => Button.onClick.RemoveListener(value);
        }

        private void OnValidate() =>
            Awake();

        private void Awake() =>
            Button ??= GetComponent<Button>();

        public void Subscribe() =>
            Button.onClick.AddListener(Call);

        public void Cleanup() =>
            Button.onClick.RemoveAllListeners();

        protected virtual void Call() { }
    }
}