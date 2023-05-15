using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Score;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons.TransitionButtons;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class LoseWindow : WindowBase
    {
        [SerializeField] private MenuButton _menuButton;
        [SerializeField] private RestartButton _restartButton;
        [SerializeField] private TextSetterAnimated _scoreText;

        private IScoreService _scoreService;
        private IPersistentProgressService _progressService;

        private int LevelId => _progressService.Progress.WorldData.LevelState.LevelId;

        public void Construct(IPersistentProgressService progressService, IScoreService scoreService,
            GameStateMachine stateMachine)
        {
            _progressService = progressService;
            _scoreService = scoreService;
            _menuButton.Construct(stateMachine);
            _restartButton.Construct(stateMachine, LevelId);
        }

        protected override void Initialize() =>
            SetScore();

        protected override void SubscribeUpdates()
        {
            _menuButton.Subscribe();
            _restartButton.Subscribe();
        }

        protected override void Cleanup()
        {
            _menuButton.Cleanup();
            _restartButton.Cleanup();
        }

        private void SetScore() =>
            _scoreText.SetTextAnimated(_scoreService.CalculateScore());
    }
}