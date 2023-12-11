using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonObserver : MonoBehaviour
    {
        [FormerlySerializedAs("_button")] [SerializeField] protected Button Button;

        private void OnValidate() =>
            Button ??= GetComponent<Button>();

        private void Awake() =>
            Button ??= GetComponent<Button>();

        public void Subscribe() =>
            Button.onClick.AddListener(Call);

        public void Unsubscribe() =>
            Button.onClick.RemoveListener(Call);

        public void Cleanup() =>
            Button.onClick.RemoveAllListeners();

        protected virtual void Call() { }
    }
}