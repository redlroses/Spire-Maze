using CodeBase.Services.Pause;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button _unpauseButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

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
            _unpauseButton.onClick.AddListener(() => _pauseService.SetPause(false));
            _menuButton.onClick.AddListener(() => print("To main menu"));
            _restartButton.onClick.AddListener(() => print("Restart level"));
        }

        protected override void CleanUp()
        {
            _unpauseButton.onClick.RemoveAllListeners();
        }
    }
}