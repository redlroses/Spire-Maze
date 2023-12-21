﻿#if !UNITY_EDITOR && UNITY_WEBGL
using CodeBase.Tools.Extension;
using Agava.YandexGames;
using Agava.WebUtility;
#endif
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.ADS;
using CodeBase.Services.Analytics;
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
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter() =>
            _sceneLoader.Load(LevelNames.Initial, EnterLoadProgress);

        public void Exit()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Get<ILocalizationService>().ChooseLanguage(YandexGamesSdk.Environment.browser.lang.AsLangId());
#else
            Get<ILocalizationService>().ChooseLanguage(LanguageId.Russian);
#endif
            Get<IRankedService>().InitLeaderboard();
            ChooseQualityLevel();
        }

        private void RegisterServices()
        {
            RegisterStaticDataService();
            Register(new RandomService());
            Register(new PersistentProgressService());
            Register(new PauseService());
            Register(new AssetProvider(Get<IPersistentProgressService>(), Get<IPauseService>()));
            Register(new CameraOperatorService());
            Register(new LocalizationService());
            Register(new AnalyticsService());
            Register(new SoundService(Get<IAssetProvider>(), Get<IPersistentProgressService>()));
            Register(new WatchService(Get<IPersistentProgressService>()));
            Register(new ScoreService(Get<IStaticDataService>(), Get<IPersistentProgressService>()));
            Register(new RankedService(Get<IStaticDataService>()));
            Register(new SaveLoadService(Get<IPersistentProgressService>(), Get<IWatchService>()));
            Register(new ADService(Get<ISoundService>(), Get<IPauseService>()));

            Register(
                new UIFactory(
                    Get<IAssetProvider>(),
                    Get<IStaticDataService>(),
                    Get<IPersistentProgressService>(),
                    Get<IRankedService>(),
                    Get<ISaveLoadService>(),
                    Get<ILocalizationService>(),
                    Get<ISoundService>(),
                    Get<IPauseService>(),
                    Get<IADService>(),
                    _stateMachine));

            Register(new WindowService(Get<IUIFactory>()));
            Register(new InputService(Get<IPauseService>()));

            Register(
                new GameFactory(
                    Get<IAssetProvider>(),
                    Get<IPersistentProgressService>(),
                    Get<IPauseService>(),
                    Get<IUIFactory>(),
                    Get<IInputService>(),
                    Get<ICameraOperatorService>()));

            Register(
                new LevelBuilder(
                    Get<IGameFactory>(),
                    Get<IStaticDataService>(),
                    _stateMachine,
                    Get<ISaveLoadService>(),
                    Get<IPersistentProgressService>(),
                    Get<IRandomService>()));
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            Register(staticData);
        }

        private void EnterLoadProgress() =>
            _stateMachine.Enter<LoadProgressState>();

        private TService Get<TService>()
            where TService : class, IService =>
            _services.Single<TService>();

        private void Register<TService>(TService service)
            where TService : class, IService =>
            _services.RegisterSingle(service);

        private void ChooseQualityLevel()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            if (Device.IsMobile)
                QualitySettings.DecreaseLevel();
#else
            QualitySettings.IncreaseLevel();
#endif
        }
    }
}