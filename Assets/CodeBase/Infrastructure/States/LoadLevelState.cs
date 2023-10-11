using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Player;
using CodeBase.Logic.StaminaEntity;
using CodeBase.Services.Cameras;
using CodeBase.Services.Input;
using CodeBase.Services.LevelBuild;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Score;
using CodeBase.Services.StaticData;
using CodeBase.Services.Watch;
using CodeBase.Tools.Extension;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using HeroInventory = CodeBase.Logic.Inventory.HeroInventory;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<LoadPayload>
    {
        private const string PlayerKey = "Player";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPlayerInputService _playerInputService;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly ILevelBuilder _levelBuilder;
        private readonly IScoreService _scoreService;
        private readonly IWatchService _watchService;
        private readonly LoadingCurtain _curtain;
        private readonly IUIFactory _uiFactory;
        private readonly IPauseService _pauseService;
        private readonly ICameraOperatorService _cameraOperatorService;

        private LoadPayload _loadPayload;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory, IPlayerInputService playerInputService,
            IUIFactory uiFactory, IPersistentProgressService progressService, IStaticDataService staticDataService,
            ILevelBuilder levelBuilder, IScoreService scoreService, IWatchService watchService, IPauseService pauseService,
            ICameraOperatorService cameraOperatorService, LoadingCurtain curtain)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _playerInputService = playerInputService;
            _uiFactory = uiFactory;
            _progressService = progressService;
            _staticData = staticDataService;
            _levelBuilder = levelBuilder;
            _scoreService = scoreService;
            _watchService = watchService;
            _pauseService = pauseService;
            _cameraOperatorService = cameraOperatorService;
            _curtain = curtain;
        }

        public void Enter(LoadPayload payload)
        {
            _curtain.Show();
            _loadPayload = payload;
            _pauseService.Cleanup();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(payload.SceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
            _levelBuilder.Clear();
        }

        private void OnLoaded()
        {
            _pauseService.SetPause(false);
            InitUIRoot();
            InitGameWorld();
            ValidateLevelProgress();
            GameObject hero = InitHero();
            CameraFollow(hero);
            InformProgressReaders();
            InitHud(hero);

            _stateMachine.Enter<GameLoopState>();
        }

        // ReSharper disable once InconsistentNaming
        private void InitUIRoot() =>
            _uiFactory.CreateUIRoot();

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);

            _watchService.LoadProgress();
            _scoreService.LoadProgress();
        }

        private void InitGameWorld()
        {
            if (_loadPayload.IsBuildable)
            {
                BuildLevel();
                ConstructLevel();
                return;
            }

            if (string.Equals(_loadPayload.SceneName, LevelNames.Lobby))
            {
                InitLobby();
                return;
            }

            if (string.Equals(_loadPayload.SceneName, LevelNames.LearningLevel))
            {
                InitLearningLevel();
            }
        }

        private void InitLearningLevel() =>
            throw new NotImplementedException();

        private void InitLobby()
        {
            GameObject lobby = _gameFactory.CreateLobby();
            EnterLevelPanel enterLevelPanel = _uiFactory.CreateEnterLevelPanel().GetComponent<EnterLevelPanel>();

            foreach (LevelTransfer levelTransfer in lobby.GetComponentsInChildren<LevelTransfer>())
            {
                levelTransfer.Construct(_stateMachine, enterLevelPanel, _progressService);
            }

            foreach (IPauseWatcher pauseWatchers in lobby.GetComponentsInChildren<IPauseWatcher>())
                _pauseService.Register(pauseWatchers);
        }

        private Vector3 GetHeroPosition() =>
            _progressService.Progress.WorldData.LevelPositions.InitialPosition.AsUnityVector();

        private void BuildLevel() =>
            _levelBuilder.Build(_staticData.ForLevel(_loadPayload.LevelId));

        private void ConstructLevel() =>
            _levelBuilder.Construct();

        private GameObject InitHero()
        {
            Vector3 heroPosition = GetHeroPosition();
            GameObject hero = _gameFactory.CreateHero(heroPosition);
            hero.GetComponent<Hero>().Construct(_playerInputService);
            hero.GetComponentInChildren<Stamina>().Construct(_staticData.StaminaForEntity(PlayerKey));
            return hero;
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            hud.GetComponent<Canvas>().worldCamera = Camera.main;
            hud.GetComponentInChildren<HealthBarView>().Construct(hero.GetComponentInChildren<IHealthReactive>());
            hud.GetComponentInChildren<StaminaBarView>().Construct(hero.GetComponentInChildren<IStamina>());
            hud.GetComponentInChildren<InventoryView>().Construct(_uiFactory, hero.GetComponent<HeroInventory>());
        }

        private void CameraFollow(GameObject hero)
        {
            if (Camera.main != null)
            {
                CameraFollower cameraFollower = Camera.main.GetComponent<CameraFollower>();
                _cameraOperatorService.RegisterCamera(cameraFollower);
                _cameraOperatorService.SetAsDefault(hero.transform);
                _cameraOperatorService.FocusOnDefault();
            }
        }

        private void ValidateLevelProgress()
        {
            if (_progressService.Progress.WorldData.LevelState.LevelId == _loadPayload.LevelId &&
                _loadPayload.IsClearLoad == false)
            {
                return;
            }

            ResetProgress();
        }

        private void ResetProgress()
        {
            _progressService.Progress.WorldData = new WorldData(null)
            {
                LevelState = new LevelState(_loadPayload.LevelId),
                LevelPositions = new LevelPositions(GetInitialPosition(), GetFinishPosition()),
                HeroHealthState = new HealthState(_staticData.HealthForEntity(PlayerKey).MaxHealth),
                HeroStaminaState = new StaminaState(_staticData.StaminaForEntity(PlayerKey).MaxStamina),
                HeroInventoryData = new InventoryData(),
                ScoreAccumulationData = new ScoreAccumulationData()
            };

            Debug.Log("Progress was reset");
        }

        private Vector3Data GetInitialPosition() =>
            _staticData.ForLevel(_loadPayload.LevelId).HeroInitialPosition.AsVectorData();

        private Vector3Data GetFinishPosition() =>
            _staticData.ForLevel(_loadPayload.LevelId).FinishPosition.AsVectorData();
    }
}