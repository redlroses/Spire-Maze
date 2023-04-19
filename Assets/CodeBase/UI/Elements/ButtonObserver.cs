using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    [RequireComponent(typeof(Button))]
    public class ButtonObserver : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        public event Action Clicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            Clicked?.Invoke();
        }
    }
}