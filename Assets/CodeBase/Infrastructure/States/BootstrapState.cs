using Agava.YandexGames;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.Cameras;
using CodeBase.Services.Input;
using CodeBase.Services.LevelBuild;
using CodeBase.Services.Localization;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.Ranked;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Score;
using CodeBase.Services.Sound;
using CodeBase.Services.StaticData;
using CodeBase.Services.Watch;
using CodeBase.Tools.Extension;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly ICoroutineRunner _coroutineRunner;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;

            RegisterServices();
        }

        public void Enter() =>
            _sceneLoader.Load(LevelNames.Initial, EnterLoadProgress);

        public void Exit()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            YandexGamesSdk.GameReady();
            _services.Single<ILocalizationService>().ChooseLanguage(YandexGamesSdk.Environment.browser.lang.AsLangId());
#endif
#if UNITY_EDITOR
            _services.Single<ILocalizationService>().ChooseLanguage(LanguageId.Russian);
#endif
            _services.Single<IRankedService>().InitLeaderboard();
        }

        private void RegisterServices()
        { 
            RegisterStaticDataService();
            _services.RegisterSingle<IRandomService>(new RandomService());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<ICameraOperatorService>(new CameraOperatorService());
            _services.RegisterSingle<ILocalizationService>(new LocalizationService());
            _services.RegisterSingle<ISoundService>(new SoundService());
            _services.RegisterSingle<IWatchService>(
                new WatchService(
                    _coroutineRunner,
                    _services.Single<IPersistentProgressService>()));
            _services.RegisterSingle<IScoreService>(
                new ScoreService(
                    _services.Single<IStaticDataService>(),
                    _services.Single<IPersistentProgressService>()));
            _services.RegisterSingle<IRankedService>(
                new RankedService(
                    _services.Single<IStaticDataService>()));
            _services.RegisterSingle<IPauseService>(
                new PauseService(_services.Single<IWatchService>()));
            _services.RegisterSingle<IUIFactory>(
                new UIFactory(
                    _services.Single<IAssetProvider>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IRankedService>(),
                    _services.Single<ILocalizationService>(),
                    _services.Single<ISoundService>(),
                    _services.Single<IPauseService>(),
                    _stateMachine));
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
            _services.RegisterSingle<IPlayerInputService>(new PlayerInputService(_services.Single<IPauseService>(),
                _services.Single<IWindowService>()));
            _services.RegisterSingle<IGameFactory>(
                new GameFactory(
                    _services.Single<IAssetProvider>(),
                    _services.Single<IRandomService>(),
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IPauseService>(),
                    _services.Single<IWindowService>(),
                    _services.Single<IWatchService>(),
                    _services.Single<IUIFactory>(),
                    _services.Single<IPlayerInputService>(),
                    _services.Single<ICameraOperatorService>()));
            _services.RegisterSingle<ISaveLoadService>(
                new SaveLoadService(
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IGameFactory>(),
                    _services.Single<IScoreService>(),
                    _services.Single<IWatchService>()));
            _services.RegisterSingle<ILevelBuilder>(
                new LevelBuilder(
                    _services.Single<IGameFactory>(),
                    _services.Single<IStaticDataService>(),
                    _stateMachine,
                    _services.Single<ISaveLoadService>(),
                    _services.Single<IPersistentProgressService>()));
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
        }

        private void EnterLoadProgress()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}