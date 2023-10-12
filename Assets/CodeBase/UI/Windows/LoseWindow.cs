using CodeBase.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
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
        [SerializeField] private TextSetterAnimated _coinsText;

        private IPersistentProgressService _progressService;

        private int LevelId => _progressService.Progress.WorldData.LevelState.LevelId;
        private WorldData WorldData => _progressService.Progress.WorldData;

        public void Construct(IPersistentProgressService progressService,
            GameStateMachine stateMachine)
        {
            _progressService = progressService;
            _menuButton.Construct(stateMachine);
            _restartButton.Construct(stateMachine, LevelId);
        }

        protected override void Initialize()
        {
            SetScore();
            SetCoins();
        }

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
            _scoreText.SetTextAnimated(WorldData._levelAccumulationData.Score);

        private void SetCoins() =>
            _coinsText.SetTextAnimated(WorldData._levelAccumulationData.Coins);
    }
}