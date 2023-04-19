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
        }

        protected override void SubscribeUpdates()
        {
            _pauseButton.onClick.AddListener(Pause);
            _unpauseButton.onClick.AddListener(Unpause);
        }

        protected override void Cleanup()
        {
            _pauseButton.onClick.RemoveListener(Pause);
            _unpauseButton.onClick.RemoveListener(Unpause);
        }

        private void Pause()
        {
            _pauseService.SetPause(true);
        }

        private void Unpause()
        {
            _pauseService.SetPause(false);
        }
    }
}