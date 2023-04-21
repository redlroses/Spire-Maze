using CodeBase.Services.Pause;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _unpauseButton;

        private IPauseService _pauseService;

        public void Construct(IPauseService pauseService)
        {
            _pauseService = pauseService;
            _pauseService.Resume += OnResume;
        }

        private void OnResume()
        {
            _pauseService.Resume -= OnResume;
            Destroy(gameObject);
        }

        protected override void SubscribeUpdates()
        {
            _pauseButton.onClick.AddListener(ActivatePause);
            _unpauseButton.onClick.AddListener(DeactivatePause);
        }

        protected override void CleanUp()
        {
            _pauseButton.onClick.RemoveListener(ActivatePause);
            _unpauseButton.onClick.RemoveListener(DeactivatePause);
        }

        private void ActivatePause()
        {
            _pauseService.SetPause(true);
        }

        private void DeactivatePause()
        {
            _pauseService.SetPause(false);
        }
    }
}