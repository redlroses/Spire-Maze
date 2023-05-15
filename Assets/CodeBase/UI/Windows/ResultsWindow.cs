using CodeBase.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons.TransitionButtons;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ResultsWindow : WindowBase
    {
        [SerializeField] private MenuButton _menuButton;
        [SerializeField] private RestartButton _restartButton;
        [SerializeField] private NextLevelButton _nextLevelButton;

        [SerializeField] private WindowAnimationPlayer _windowAnimationPlayer;
        [SerializeField] private TextSetterAnimated _scoreText;
        [SerializeField] private TextSetterAnimated _itemText;
        [SerializeField] private StarsView _starsView;

        private IPersistentProgressService _progressService;

        private int LevelId => _progressService.Progress.WorldData.LevelState.LevelId;
        private WorldData WorldData => _progressService.Progress.WorldData;

        public void Construct(IPersistentProgressService progressService, GameStateMachine stateMachine)
        {
            _progressService = progressService;
            _restartButton.Construct(stateMachine);
            _nextLevelButton.Construct(stateMachine);
            _menuButton.Construct(stateMachine);
            _restartButton.Construct(stateMachine, LevelId);
            _nextLevelButton.Construct(stateMachine, LevelId);
        }

        protected override void Initialize()
        {
            _windowAnimationPlayer.Play();
            var worldData = WorldData;
            SetScorePoints(worldData);
            SetItemsCount(worldData);
            SetStars(worldData);
        }

        protected override void SubscribeUpdates()
        {
            _menuButton.Subscribe();
            _restartButton.Subscribe();
            _nextLevelButton.Subscribe();
        }

        protected override void Cleanup()
        {
            _menuButton.Cleanup();
            _restartButton.Cleanup();
            _nextLevelButton.Cleanup();
        }

        private void SetStars(WorldData worldData) =>
            _starsView.EnableStars(worldData.ScoreAccumulationData.LevelStars);

        private void SetItemsCount(WorldData worldData) =>
            _itemText.SetTextAnimated(worldData.ScoreAccumulationData.Artifacts);

        private void SetScorePoints(WorldData worldData) =>
            _scoreText.SetTextAnimated(worldData.ScoreAccumulationData.LevelScore);
    }
}