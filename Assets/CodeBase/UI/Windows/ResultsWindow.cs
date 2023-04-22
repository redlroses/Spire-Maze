using System;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Score;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
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
        [SerializeField] private TextSetter _scoreText;
        [SerializeField] private StarsView _starsView;

        private IScoreService _scoreCounter;
        private IPersistentProgressService _progressService;
        private IStaticDataService _staticDataService;
        private GameStateMachine _stateMachine;

        public void Construct(IPersistentProgressService progressService, IScoreService scoreCounter,
            IStaticDataService staticDataService, GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _progressService = progressService;
            _scoreCounter = scoreCounter;
        }

        protected override void Initialize()
        {
            _windowAnimationPlayer.Play();
            _scoreText.SetText(_scoreCounter.CalculateScore());
            ScoreConfig scoreConfig = GetScoreConfig();
            _starsView.EnableStars(StarsCountFromConfig(scoreConfig));
        }

        protected override void SubscribeUpdates()
        {
            _menuButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.TestLevelName, true,
                    LevelNames.FirstLevelKey)));
            _restartButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.TestLevelName, true,
                    LevelNames.FirstLevelKey, true)));
            _nextLevelButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.TestLevelName, true,
                    LevelNames.FirstLevelKey)));
        }

        private ScoreConfig GetScoreConfig() =>
            _staticDataService.ScoreForLevel(_progressService.Progress.WorldData.LevelState.LevelKey);

        private int StarsCountFromConfig(ScoreConfig scoreConfig) =>
            Convert.ToInt32(scoreConfig.RaitingCurve.Evaluate(_scoreCounter.CurrentScore));
    }
}