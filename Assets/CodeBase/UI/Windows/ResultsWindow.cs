using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class ResultsWindow : WindowBase
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextLevelButton;

        [SerializeField] private WindowAnimationPlayer _windowAnimationPlayer;
        [SerializeField] private TextSetterAnimated _scoreText;
        [SerializeField] private TextSetterAnimated _itemText;
        [SerializeField] private StarsView _starsView;

        private IPersistentProgressService _progressService;
        private GameStateMachine _stateMachine;

        private int LevelId => _progressService.Progress.WorldData.LevelState.LevelId;
        private WorldData WorldData => _progressService.Progress.WorldData;

        public void Construct(IPersistentProgressService progressService, GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
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
            _menuButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(MenuPayload()));
            _restartButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.BuildableLevel, true,
                    LevelId, true)));
            _nextLevelButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.BuildableLevel, true,
                    LevelId + 1)));
        }

        private static LoadPayload MenuPayload() =>
            new LoadPayload(LevelNames.Lobby, false,
                LevelNames.LobbyId);

        private void SetStars(WorldData worldData) =>
            _starsView.EnableStars(worldData.ScoreAccumulationData.LevelStars);

        private void SetItemsCount(WorldData worldData) =>
            _itemText.SetTextAnimated(worldData.ScoreAccumulationData.Artifacts);

        private void SetScorePoints(WorldData worldData) =>
            _scoreText.SetTextAnimated(worldData.ScoreAccumulationData.LevelScore);
    }
}