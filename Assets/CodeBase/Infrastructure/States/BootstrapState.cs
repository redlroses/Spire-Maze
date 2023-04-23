using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
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
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter() =>
            _sceneLoader.Load(LevelNames.Initial, onLoaded: EnterLoadLevel);

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            RegisterStaticDataService();
            _services.RegisterSingle<IRandomService>(new RandomService());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IPauseService>(new PauseService());
            _services.RegisterSingle<IGameFactory>(
                new GameFactory(
                    _services.Single<IAssetProvider>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<IRandomService>(),
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IPauseService>()));
            _services.RegisterSingle<ILocalizationService>(new LocalizationService());
            _services.RegisterSingle<ISoundService>(new SoundService());
            _services.RegisterSingle<IScoreService>(new ScoreService(_services.Single<IPersistentProgressService>(),
                _services.Single<IStaticDataService>()));
            _services.RegisterSingle<IRankedService>(new RankedService(_services.Single<IStaticDataService>()));
            _services.RegisterSingle<IUIFactory>(
                new UIFactory(
                    _services.Single<IAssetProvider>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IRankedService>(),
                    _services.Single<ILocalizationService>(),
                    _services.Single<ISoundService>(),
                    _services.Single<IPauseService>(),
                    _services.Single<IScoreService>(),
                    _stateMachine));
            _services.RegisterSingle<ISaveLoadService>(
                new SaveLoadService(
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IGameFactory>()));
            _services.RegisterSingle<ILevelBuilder>(
                new LevelBuilder(
                    _services.Single<IGameFactory>(),
                    _services.Single<IStaticDataService>()));
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}