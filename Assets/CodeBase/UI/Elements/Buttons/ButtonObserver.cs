using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons
{
    [RequireComponent(typeof(Button))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class ButtonObserver : MonoBehaviour
    {
        [SerializeField] private Button _button;

        protected Button Button => _button;

        private void Awake() =>
            _button ??= GetComponent<Button>();

        private void OnValidate() =>
            _button ??= GetComponent<Button>();

        public void Subscribe() =>
            Button.onClick.AddListener(Call);

        public void Unsubscribe() =>
            Button.onClick.RemoveListener(Call);

        public void Cleanup() =>
            Button.onClick.RemoveAllListeners();

        protected virtual void Call()
        {
        }
    }
}