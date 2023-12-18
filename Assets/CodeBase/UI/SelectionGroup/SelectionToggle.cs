using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.SelectionGroup
{
    public abstract class SelectionToggle<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private TEnum _id;
        [SerializeField] private Image _checkMark;
        [SerializeField] private Button _button;

        public event Action<SelectionToggle<TEnum>> Selected;

        public TEnum Id => _id;

        private void Awake() =>
            _button.onClick.AddListener(OnSelected);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnSelected);

        public void Unselect() =>
            _checkMark.enabled = false;

        public void Select() =>
            _checkMark.enabled = true;

        private void OnSelected() =>
            Selected?.Invoke(this);
    }
}