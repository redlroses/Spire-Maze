using CodeBase.Services.Pause;
using CodeBase.UI.Services.Windows;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class PauseToggle : MonoBehaviour
    {
        [SerializeField] private BetterToggle _pauseToggle;

        private IPauseService _pauseService;
        private IWindowService _windowService;

        public void Construct(IPauseService pauseService, IWindowService windowService)
        {
            _windowService = windowService;
            _pauseService = pauseService;
            Subscribe();
        }

        private void Subscribe()
        {
            _pauseToggle.onValueChanged.AddListener(OnPauseToggle);
        }

        private void OnPauseToggle(bool isPause)
        {
            _pauseService.SetPause(isPause);

            if (isPause)
            {
                _windowService.Open(WindowId.Pause);
            }
        }

        private void OnDestroy()
        {
            _pauseToggle.onValueChanged.RemoveAllListeners();
        }
    }
}