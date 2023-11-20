using CodeBase.Services.Pause;
using CodeBase.UI.Services.Windows;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class PauseToggle : MonoBehaviour, IPauseWatcher
    {
        [SerializeField] private BetterToggle _pauseToggle;
        [SerializeField] private LocationAnimations _pauseButtonAnimation;

        private IPauseService _pauseService;

        private void OnDestroy() =>
            _pauseToggle.onValueChanged.RemoveAllListeners();

        public void Construct(IPauseService pauseService)
        {
            _pauseService = pauseService;
            Subscribe();
        }

        public void EmulateClick() =>
            _pauseToggle.isOn = !_pauseToggle.isOn;

        public void Resume() =>
            _pauseToggle.isOn = false;

        public void Pause() =>
            _pauseToggle.isOn = true;

        private void Subscribe() =>
            _pauseToggle.onValueChanged.AddListener(OnPauseToggle);

        private void OnPauseToggle(bool isPause)
        {
            if (_pauseService.IsPause)
                return;

            _pauseService.SetPause(isPause);
        }
    }
}