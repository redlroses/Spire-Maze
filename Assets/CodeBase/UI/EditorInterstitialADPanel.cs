using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class EditorInterstitialADPanel : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _errorButton;

        private void Awake()
        {
            _closeButton.interactable = true;
        }

        public void Open(Action<bool> onCloseCallback, Action<string> onErrorCallback)
        {
            _errorButton.onClick.AddListener(() =>
            {
                onErrorCallback.Invoke(string.Empty);
                Destroy(gameObject);
            });
            _closeButton.onClick.AddListener(() => onCloseCallback.Invoke(true));
            _closeButton.onClick.AddListener(() => Destroy(gameObject));
        }
    }
}