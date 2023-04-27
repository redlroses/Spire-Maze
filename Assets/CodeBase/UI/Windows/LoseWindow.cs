using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Score;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class LoseWindow : WindowBase
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextSetterAnimated _scoreText;
        
        private GameStateMachine _stateMachine;
        private IScoreService _scoreService;
        private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService, IScoreService scoreService, GameStateMachine stateMachine)
        {
            _progressService = progressService;
            _stateMachine = stateMachine;
            _scoreService = scoreService;
        }
        
        protected override void Initialize()
        {
            _scoreText.SetTextAnimated(_scoreService.CalculateScore());
        }
        
        protected override void SubscribeUpdates()
        {
            _menuButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.Lobby, false,
                    LevelNames.LobbyId)));
            _restartButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.BuildableLevel, true,
                    _progressService.Progress.WorldData.LevelState.LevelId + 1, true)));
        }
    }
}