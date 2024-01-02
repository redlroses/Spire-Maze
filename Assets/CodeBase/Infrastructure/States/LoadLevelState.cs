#if !UNITY_EDITOR && YANDEX_GAMES
using Agava.YandexGames;
#endif
using System;
using System.Collections.Generic;
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
using CodeBase.Services.Analytics;
using CodeBase.Services.Cameras;
using CodeBase.Services.Input;
using CodeBase.Services.LevelBuild;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Sound;
using CodeBase.Services.StaticData;
using CodeBase.Services.Watch;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;
using CodeBase.Tutorial;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable CS4014

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<LoadPayload>
    {
        private const string PlayerKey = "Player";
        private const int DefaultLoadDelay = 1;
        private const int FirstLoadDelay = 2;

        private readonly IADService _adService;
        private readonly IAnalyticsService _analytics;
        private readonly ICameraOperatorService _cameraOperatorService;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;
        private readonly ILevelBuilder _levelBuilder;
        private readonly MeshCombiner _meshCombiner;
        private readonly IPauseService _pauseService;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SceneLoader _sceneLoader;
        private readonly ISoundService _soundService;
        private readonly GameStateMachine _stateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;
        private readonly IWatchService _watchService;
        private readonly IWindowService _windowService;
        private bool _isFirstLoad = true;

        private LoadPayload _loadPayload;

        public LoadLevelState(
            GameStateMachine gameStateMachine,
            IADService adService,
            IAnalyticsService analytics,
            ICameraOperatorService cameraOperatorService,
            IGameFactory gameFactory,
            IInputService inputService,
            IUIFactory uiFactory,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            IStaticDataService staticDataService,
            ILevelBuilder levelBuilder,
            IWatchService watchService,
            IPauseService pauseService,
            IWindowService windowService,
            ISoundService soundService,
            SceneLoader sceneLoader,
            LoadingCurtain curtain)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _inputService = inputService;
            _uiFactory = uiFactory;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _staticData = staticDataService;
            _levelBuilder = levelBuilder;
            _watchService = watchService;
            _pauseService = pauseService;
            _analytics = analytics;
            _cameraOperatorService = cameraOperatorService;
            _windowService = windowService;
            _adService = adService;
            _soundService = soundService;
            _curtain = curtain;
            _meshCombiner = new MeshCombiner();
        }

        public void Enter(LoadPayload loadPayload)
        {
            _curtain.Show();

            _loadPayload = loadPayload;

            if (loadPayload.IsClearLoad)
            {
                _analytics.TrackLevelStart(loadPayload.LevelId);
                _watchService.Cleanup();
            }

            _pauseService.Cleanup();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(loadPayload.SceneName, () => OnLoaded());
        }

        public void Exit()
        {
            _levelBuilder.Clear();

            if (_loadPayload.IsSaveAfterLoad)
            {
                _progressService.Progress.WorldData.SceneName = GetCurrentScene();
                _saveLoadService.SaveProgress();
            }

            if (_isFirstLoad)
            {
                _curtain.Hide(
                    FirstLoadDelay,
                    () =>
                    {
#if !UNITY_EDITOR && YANDEX_GAMES
                        YandexGamesSdk.GameReady();
#endif
                    });

                _isFirstLoad = false;
            }
            else
            {
                _curtain.Hide(DefaultLoadDelay);
            }

            if (_loadPayload.IsShowAd)
                _adService.ShowInterstitialAd();
        }

        private async UniTaskVoid OnLoaded()
        {
            _gameFactory.CreateCamera();
            await UniTask.DelayFrame(2);
            InitUIRoot();
            await InitGameWorld();
            ValidateLevelProgress();
            GameObject hero = InitHero();
            InitReviver(hero);
            CameraFollow(hero);
            InformProgressReaders();
            InitHud(hero);
            InitLevelNamePanel();
            RegisterServicesInPauseService();
            InitMusicPlayer();

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
            _progressService.InformReaders();
            _watchService.LoadProgress();
        }

        private async UniTask InitGameWorld()
        {
            switch (_loadPayload.SceneName)
            {
                case LevelNames.BuildableLevel:
                    await CreateBuildableLevel();

                    return;
                case LevelNames.Lobby:
                    await InitLobby();

                    return;
                case LevelNames.LearningLevel:
                    await InitLearningLevel();

                    return;
                default:
                    throw new Exception($"Unknown level name {_loadPayload.SceneName}");
            }
        }

        private async UniTask<Level> CreateBuildableLevel()
        {
            Level level = BuildLevel();
            await ConstructLevel();

            return level;
        }

        private async UniTask InitLearningLevel()
        {
            TutorialConfig config = _staticData.GetTutorialConfig();
            Level level = await CreateBuildableLevel();
            IReadOnlyCollection<TutorialTrigger> triggers = SpawnTutorialTriggers(level, config);
            GameObject sequence = _uiFactory.CreateTutorialSequence();
            sequence.GetComponent<TutorialSequence>().Construct(_inputService, config, triggers);
        }

        private IReadOnlyCollection<TutorialTrigger> SpawnTutorialTriggers(Level level, TutorialConfig config)
        {
            List<TutorialTrigger> triggers = new List<TutorialTrigger>(config.ModulesLength);

            foreach (TutorialModule module in config.Modules)
            {
                Transform triggerContainer = level.GetCell(module.CellIndex).Container;
                GameObject trigger = _gameFactory.CreateTutorialTrigger(triggerContainer);
                triggers.Add(trigger.GetComponent<TutorialTrigger>());
            }

            return triggers;
        }

        private async UniTask InitLobby()
        {
            GameObject lobby = _gameFactory.CreateLobby();
            await _meshCombiner.CombineAllMeshes(lobby.transform, _gameFactory.CreateMaterial(AssetPath.SpireMaterial));
            EnterLevelPanel enterLevelPanel = _uiFactory.CreateEnterLevelPanel().GetComponent<EnterLevelPanel>();

            foreach (LevelTransfer levelTransfer in lobby.GetComponentsInChildren<LevelTransfer>())
                levelTransfer.Construct(_stateMachine, enterLevelPanel, _progressService);

            await UniTask.Yield();
            InitLobbyDoors(lobby);
            await UniTask.Yield();

            foreach (IPauseWatcher pauseWatchers in lobby.GetComponentsInChildren<IPauseWatcher>())
                _pauseService.Register(pauseWatchers);
        }

        private void InitLobbyDoors(GameObject lobby)
        {
            int lastCompletedLevelId = 0;

            if (_progressService.Progress.GlobalData.Levels.Any())
                lastCompletedLevelId = _progressService.Progress.GlobalData.Levels.Max(level => level.Id);

            foreach (LobbyDoor lobbyDoor in lobby.GetComponentsInChildren<LobbyDoor>())
                lobbyDoor.Construct(lastCompletedLevelId);
        }

        private Vector3 GetHeroPosition() =>
            _progressService.Progress.WorldData.LevelPositions.InitialPosition.AsUnityVector();

        private Level BuildLevel() =>
            _levelBuilder.Build(_staticData.GetLevel(_loadPayload.LevelId));

        private async UniTask ConstructLevel() =>
            await _levelBuilder.ConstructLevel();

        private GameObject InitHero()
        {
            Vector3 heroPosition = GetHeroPosition();
            GameObject hero = _gameFactory.CreateHero(heroPosition);
            hero.GetComponent<HeroRoot>().Construct(_inputService, _stateMachine);
            hero.GetComponentInChildren<Stamina>().Construct(_staticData.GetStaminaEntity(PlayerKey));

            return hero;
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            hud.GetComponent<Canvas>().worldCamera = Camera.main;
            hud.GetComponentInChildren<PauseToggle>().Construct(_pauseService);
            hud.GetComponentInChildren<ClockText>().Construct(_watchService);
            hud.GetComponentInChildren<ExtraLivesBarView>().Construct(_uiFactory);
            hud.GetComponentInChildren<MuteButton>().Construct(_soundService);
            hud.GetComponentInChildren<TutorialButton>().Construct(_windowService);
            hud.GetComponentInChildren<LeaderboardButton>().Construct(_windowService);
            hud.GetComponentInChildren<SettingsButton>().Construct(_windowService);
            hud.GetComponentInChildren<HealthBarView>().Construct(hero.GetComponentInChildren<IHealthReactive>());
            hud.GetComponentInChildren<StaminaBarView>().Construct(hero.GetComponentInChildren<IStamina>());
            hud.GetComponentInChildren<InventoryView>().Construct(_uiFactory, hero.GetComponent<HeroInventory>());
            hud.GetComponentInChildren<ItemCollectedView>().Construct(hero.GetComponent<ItemCollector>());
        }

        private void InitLevelNamePanel()
        {
            LevelNamePanel levelNamePanel = _uiFactory.CreateLevelNamePanel().GetComponent<LevelNamePanel>();

            int starsCount = _progressService.Progress.GlobalData.Levels.Find(level => level.Id == _loadPayload.LevelId)
                ?.Stars ?? 0;

            levelNamePanel.Show(starsCount, _loadPayload.LevelId);
        }

        private void InitMusicPlayer()
        {
            if (_isFirstLoad)
                _gameFactory.CreateMusicPlayer();
        }

        private void CameraFollow(GameObject hero)
        {
            CameraFollower cameraFollower = Camera.main!.GetComponent<CameraFollower>();
            cameraFollower.Construct(_staticData);
            _cameraOperatorService.RegisterCamera(cameraFollower);
            _cameraOperatorService.SetAsDefault(hero.transform);
            _cameraOperatorService.FocusOnDefault();
        }

        private void RegisterServicesInPauseService()
        {
            _pauseService.Register(_windowService as IPauseWatcher);
            _pauseService.Register(_watchService as IPauseWatcher);
        }

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
            _progressService.Progress.WorldData =
                new WorldData(_progressService.Progress.WorldData.SceneName, _loadPayload.LevelId)
                {
                    LevelPositions = new LevelPositions(GetInitialPosition(), GetFinishPosition()),
                    HeroHealthState = new HealthState(_staticData.GetHealthEntity(PlayerKey).MaxHealth),
                    HeroStaminaState = new StaminaState(_staticData.GetStaminaEntity(PlayerKey).MaxStamina),
                    HeroInventoryData = new InventoryData(),
                    AccumulationData = new AccumulationData(),
                };
        }

        private Vector3Data GetInitialPosition() =>
            _staticData.GetLevel(_loadPayload.LevelId).HeroInitialPosition.AsVectorData();

        private Vector3Data GetFinishPosition() =>
            _staticData.GetLevel(_loadPayload.LevelId).FinishPosition.AsVectorData();

        private string GetCurrentScene() =>
            SceneManager.GetActiveScene().name;
    }
}