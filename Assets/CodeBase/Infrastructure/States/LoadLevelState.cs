#if !UNITY_EDITOR && YANDEX_GAMES
using Agava.YandexGames;
#endif
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
using CodeBase.Services.Cameras;
using CodeBase.Services.Input;
using CodeBase.Services.LevelBuild;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
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
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticData;
        private readonly ILevelBuilder _levelBuilder;
        private readonly IWatchService _watchService;
        private readonly LoadingCurtain _curtain;
        private readonly IUIFactory _uiFactory;
        private readonly IPauseService _pauseService;
        private readonly ICameraOperatorService _cameraOperatorService;
        private readonly IWindowService _windowService;
        private readonly IADService _adService;
        private readonly MeshCombiner _meshCombiner;

        private LoadPayload _loadPayload;
        private bool _isFirstLoad = true;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory, IInputService inputService,
            IUIFactory uiFactory, IPersistentProgressService progressService, ISaveLoadService saveLoadService, IStaticDataService staticDataService,
            ILevelBuilder levelBuilder, IWatchService watchService, IPauseService pauseService,
            ICameraOperatorService cameraOperatorService, IWindowService windowService, IADService adService, LoadingCurtain curtain)
        {
            _adService = adService;
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

            if (_loadPayload.IsSaveAfterLoad)
            {
                _progressService.Progress.WorldData.SceneName = GetCurrentScene();
                _saveLoadService.SaveProgress();
            }

#if !UNITY_EDITOR && YANDEX_GAMES
            if (_isFirstLoad)
                YandexGamesSdk.GameReady();
#endif
            _isFirstLoad = false;
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

        private void InitGameWorld()
        {
            switch (_loadPayload.SceneName)
            {
                case LevelNames.BuildableLevel:
                    CreateBuildableLevel();
                    return;
                case LevelNames.Lobby:
                    InitLobby();
                    return;
                case LevelNames.LearningLevel:
                    InitLearningLevel();
                    return;
                default:
                    throw new System.Exception($"Unknown level name {_loadPayload.SceneName}");
            }
        }

        private Level CreateBuildableLevel()
        {
            Level level = BuildLevel();
            ConstructLevel();
            return level;
        }

        private void InitLearningLevel()
        {
            TutorialConfig config = _staticData.GetTutorialConfig();
            Level level = CreateBuildableLevel();
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
            _levelBuilder.Build(_staticData.GetLevel(_loadPayload.LevelId));

        private void ConstructLevel() =>
            _levelBuilder.Construct();

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
            hud.GetComponentInChildren<LeaderboardButton>().Construct(_windowService);
            hud.GetComponentInChildren<SettingsButton>().Construct(_windowService);
            hud.GetComponentInChildren<HealthBarView>().Construct(hero.GetComponentInChildren<IHealthReactive>());
            hud.GetComponentInChildren<StaminaBarView>().Construct(hero.GetComponentInChildren<IStamina>());
            hud.GetComponentInChildren<InventoryView>().Construct(_uiFactory, hero.GetComponent<HeroInventory>());
            hud.GetComponentInChildren<ItemCollectedView>().Construct(hero.GetComponent<ItemCollector>());
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
                HeroHealthState = new HealthState(_staticData.GetHealthEntity(PlayerKey).MaxHealth),
                HeroStaminaState = new StaminaState(_staticData.GetStaminaEntity(PlayerKey).MaxStamina),
                HeroInventoryData = new InventoryData(),
                AccumulationData = new AccumulationData()
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