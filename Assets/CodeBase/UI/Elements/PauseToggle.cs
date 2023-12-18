using CodeBase.Services.Input;
using CodeBase.Services.Pause;
using CodeBase.Tools;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class PauseToggle : MonoBehaviour, IPauseWatcher
    {
        private readonly Locker _inputLocker = new Locker(nameof(InputService));

        [SerializeField] private BetterToggle _pauseToggle;

        private IPauseService _pauseService;

        private void OnDestroy() =>
            _pauseToggle.onValueChanged.RemoveAllListeners();

        public void Construct(IPauseService pauseService)
        {
            _pauseService = pauseService;
            Subscribe();
        }

        public void EmulateClick() =>
            _pauseToggle.SetIsOnWithoutNotify(!_pauseToggle.isOn);

        public void Resume() =>
            _pauseToggle.isOn = false;

        public void Pause() =>
            _pauseToggle.isOn = true;

        private void Subscribe() =>
            _pauseToggle.onValueChanged.AddListener(OnPauseToggle);

        private void OnPauseToggle(bool isPause)
        {
            if (isPause)
                _pauseService.EnablePause(_inputLocker);
            else
                _pauseService.DisablePause(_inputLocker);
        }
    }
}