using System;
using CodeBase.DelayRoutines;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class EditorRewardADPanel : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _errorButton;
        [SerializeField] private TextSetter _timer;

        [SerializeField] private int _showTime;

        private int _count;

        private void Awake()
        {
            _timer.SetText(_count = _showTime);
            _closeButton.interactable = false;
        }

        public void Open(Action onRewardedCallback, Action onCloseCallback, Action<string> onErrorCallback)
        {
            new RoutineSequence().WaitForSeconds(1f).Then(() => _timer.SetText(--_count))
                .LoopWhile(() => _count > 0).Then(() =>
                {
                    onRewardedCallback?.Invoke();
                    _closeButton.interactable = true;
                }).SetAutoKill(true).Play();

            _errorButton.onClick.AddListener(() =>
            {
                onErrorCallback.Invoke(string.Empty);
                Destroy(gameObject);
            });
            _closeButton.onClick.AddListener(onCloseCallback.Invoke);
            _closeButton.onClick.AddListener(() => Destroy(gameObject));
        }
    }
}