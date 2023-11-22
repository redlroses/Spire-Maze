using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification;
using CodeBase.Logic;
using CodeBase.Logic.Cameras;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Inventory;
using CodeBase.Logic.StaminaEntity;
using CodeBase.Logic.Сollectible;
using CodeBase.MeshCombine;
using CodeBase.Services.ADS;
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
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<LoadPayload>
    {
        private const string PlayerKey = "Player";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly ILevelBuilder _levelBuilder;
        private readonly IScoreService _scoreService;
        private readonly IWatchService _watchService;
        private readonly LoadingCurtain _curtain;
        private readonly IUIFactory _uiFactory;
        private readonly IPauseService _pauseService;
        private readonly ICameraOperatorService _cameraOperatorService;
        private readonly IWindowService _windowService;
        private readonly IADService _adService;
        private readonly MeshCombiner _meshCombiner;

        private LoadPayload _loadPayload;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory, IInputService inputService,
            IUIFactory uiFactory, IPersistentProgressService progressService, IStaticDataService staticDataService,
            ILevelBuilder levelBuilder, IScoreService scoreService, IWatchService watchService, IPauseService pauseService,
            ICameraOperatorService cameraOperatorService, IWindowService windowService, IADService adService, LoadingCurtain curtain)
        {
            _adService = adService;
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _inputService = inputService;
            _uiFactory = uiFactory;
            _progressService = progressService;
            _staticData = staticDataService;
            _levelBuilder = levelBuilder;
            _scoreService = scoreService;
            _watchService = watchService;
            _pauseService = pauseService;
            _cameraOperatorService = cameraOperatorService;
            _windowService = windowService;
            _curtain = curtain;
            _meshCombiner = new MeshCombiner();
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
            _adService.ShowInterstitialAd();
        }

        private void OnLoaded()
        {
            _pauseService.SetPause(false);
            InitUIRoot();
            InitGameWorld();
            ValidateLevelProgress();
            GameObject hero = InitHero();
            InitReviver(hero);
            CameraFollow(hero);
            InformProgressReaders();
            InitHud(hero);
            RegisterWindowsServiceInPauseService();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitReviver(GameObject hero)
        {
            HeroReviver reviver = hero.GetComponent<HeroReviver>();
            _uiFactory.Init(reviver);
        }

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
                CreateLevel();
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

        private Level CreateLevel()
        {
            Level level = BuildLevel();
            ConstructLevel();
            return level;
        }

        private void InitLearningLevel()
        {
            Level level = CreateLevel();
        }

        private void InitLobby()
        {
            GameObject lobby = _gameFactory.CreateLobby();
            _meshCombiner.CombineAllMeshes(lobby.transform, _gameFactory.CreateMaterial(AssetPath.SpireMaterial));
            EnterLevelPanel enterLevelPanel = _uiFactory.CreateEnterLevelPanel().GetComponent<EnterLevelPanel>();

            foreach (LevelTransfer levelTransfer in lobby.GetComponentsInChildren<LevelTransfer>())
                levelTransfer.Construct(_stateMachine, enterLevelPanel, _progressService);

            InitLobbyDoors(lobby);

            foreach (IPauseWatcher pauseWatchers in lobby.GetComponentsInChildren<IPauseWatcher>())
                _pauseService.Register(pauseWatchers);
        }

        private void InitLobbyDoors(GameObject lobby)
        {
            int lastCompletedLevelId = 0;

            if (_progressService.Progress.GlobalData.Levels.Any())
            {
                lastCompletedLevelId = _progressService.Progress.GlobalData.Levels.Max(level => level.Id);
            }

            foreach (LobbyDoor lobbyDoor in lobby.GetComponentsInChildren<LobbyDoor>())
            {
                lobbyDoor.Construct(lastCompletedLevelId);
            }
        }

        private Vector3 GetHeroPosition() =>
            _progressService.Progress.WorldData.LevelPositions.InitialPosition.AsUnityVector();

        private Level BuildLevel() =>
            _levelBuilder.Build(_staticData.GetForLevel(_loadPayload.LevelId));

        private void ConstructLevel() =>
            _levelBuilder.Construct();

        private GameObject InitHero()
        {
            Vector3 heroPosition = GetHeroPosition();
            GameObject hero = _gameFactory.CreateHero(heroPosition);
            hero.GetComponent<HeroRoot>().Construct(_inputService, _stateMachine);
            hero.GetComponentInChildren<Stamina>().Construct(_staticData.GetStaminaForEntity(PlayerKey));
            return hero;
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            hud.GetComponent<Canvas>().worldCamera = Camera.main;
            hud.GetComponentInChildren<HealthBarView>().Construct(hero.GetComponentInChildren<IHealthReactive>());
            hud.GetComponentInChildren<StaminaBarView>().Construct(hero.GetComponentInChildren<IStamina>());
            hud.GetComponentInChildren<InventoryView>().Construct(_uiFactory, hero.GetComponent<HeroInventory>());
            hud.GetComponentInChildren<ItemCollectedView>().Construct(hero.GetComponent<ItemCollector>());
        }

        private void CameraFollow(GameObject hero)
        {
            CameraFollower cameraFollower = Camera.main!.GetComponent<CameraFollower>();
            cameraFollower.Construct(_staticData);
            _cameraOperatorService.RegisterCamera(cameraFollower);
            _cameraOperatorService.SetAsDefault(hero.transform);
            _cameraOperatorService.FocusOnDefault();
        }

        private void RegisterWindowsServiceInPauseService() =>
            _pauseService.Register(_windowService as IPauseWatcher);

        private void ValidateLevelProgress()
        {
            if (_progressService.Progress.WorldData.LevelState.LevelId != _loadPayload.LevelId ||
                _loadPayload.IsClearLoad)
            {
                ResetProgress();
            }
        }

        private void ResetProgress()
        {
            _progressService.Progress.WorldData = new WorldData(_progressService.Progress.WorldData.SceneName, _loadPayload.LevelId)
            {
                LevelPositions = new LevelPositions(GetInitialPosition(), GetFinishPosition()),
                HeroHealthState = new HealthState(_staticData.GetHealthForEntity(PlayerKey).MaxHealth),
                HeroStaminaState = new StaminaState(_staticData.GetStaminaForEntity(PlayerKey).MaxStamina),
                HeroInventoryData = new InventoryData(),
                LevelAccumulationData = new LevelAccumulationData()
            };

            Debug.Log("Progress was reset");
        }

        private Vector3Data GetInitialPosition() =>
            _staticData.GetForLevel(_loadPayload.LevelId).HeroInitialPosition.AsVectorData();

        private Vector3Data GetFinishPosition() =>
            _staticData.GetForLevel(_loadPayload.LevelId).FinishPosition.AsVectorData();
    }
}