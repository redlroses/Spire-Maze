using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.UI.Elements.Buttons
{
    public class LeaderboardButton : ButtonObserver
    {
        [SerializeField] private PauseToggle _pauseToggle;

        private IWindowService _windowService;

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
            Subscribe();
        }

        private void OnDestroy() =>
            Cleanup();

        protected override void Call()
        {
            _windowService.Open(WindowId.Leaderboard);
            _pauseToggle.EmulateClick();
        }
    }
}