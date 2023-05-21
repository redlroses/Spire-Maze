using CodeBase.UI.Services.Windows;

namespace CodeBase.UI.Elements.Buttons
{
    public class LeaderboardButton : ButtonObserver
    {
        private IWindowService _windowService;

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
            Subscribe();
        }

        private void OnDestroy() =>
            Cleanup();

        protected override void Call() =>
            _windowService.Open(WindowId.Leaderboard);
    }
}