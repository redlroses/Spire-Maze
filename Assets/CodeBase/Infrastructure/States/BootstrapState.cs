#if !UNITY_EDITOR && UNITY_WEBGL
using System.Threading.Tasks;
#endif
using CodeBase.Tools.Extension;
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
using Cysharp.Threading.Tasks;
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
            ApplyLanguage();
            ChooseQualityLevel();
            Get<IRankedService>().InitLeaderboard();
        }

        private void RegisterServices()
        {
            RegisterStaticDataService();
            Register<IRandomService>(new RandomService());
            Register<IPersistentProgressService>(new PersistentProgressService());
            Register<IPauseService>(new PauseService(_stateMachine));
            Register<IAssetProvider>(new AssetProvider(Get<IPersistentProgressService>(), Get<IPauseService>()));
            Register<ICameraOperatorService>(new CameraOperatorService());
            Register<ILocalizationService>(new LocalizationService());
            Register<IAnalyticsService>(new AnalyticsService());
            Register<ISoundService>(new SoundService(Get<IAssetProvider>(), Get<IPersistentProgressService>()));
            Register<IWatchService>(new WatchService(Get<IPersistentProgressService>()));
            Register<IScoreService>(new ScoreService(Get<IStaticDataService>(), Get<IPersistentProgressService>()));
            Register<IRankedService>(new RankedService(Get<IStaticDataService>()));
            Register<ISaveLoadService>(new SaveLoadService(Get<IPersistentProgressService>(), Get<IWatchService>()));
            Register<IADService>(new ADService(Get<ISoundService>(), Get<IPauseService>()));

            Register<IUIFactory>(
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

            Register<IWindowService>(new WindowService(Get<IUIFactory>()));
            Register<IInputService>(new InputService(Get<IPauseService>()));

            Register<IGameFactory>(
                new GameFactory(
                    Get<IAssetProvider>(),
                    Get<IPersistentProgressService>(),
                    Get<IUIFactory>(),
                    Get<IInputService>(),
                    Get<ICameraOperatorService>()));

            Register<ILevelBuilder>(
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

        private async UniTaskVoid ChooseQualityLevel()
        {
            if (await IsMobile())
                QualitySettings.DecreaseLevel();
            else
                QualitySettings.IncreaseLevel();
        }

        private async UniTask<bool> IsMobile()
        {
#if !UNITY_EDITOR && UNITY_WEBGL && YANDEX_GAMES
            return Agava.WebUtility.Device.IsMobile;
#elif !UNITY_EDITOR && UNITY_WEBGL && CRAZY_GAMES
            return await IsMobileOnCrazyGames();
#else
            return false;
#endif
        }

        private async UniTaskVoid ApplyLanguage() =>
            Get<ILocalizationService>().ChooseLanguage(await GetLanguageId());

        private async UniTask<LanguageId> GetLanguageId()
        {
#if !UNITY_EDITOR && UNITY_WEBGL && YANDEX_GAMES
            return Agava.YandexGames.YandexGamesSdk.Environment.browser.lang.AsLangId();
#elif !UNITY_EDITOR && UNITY_WEBGL && CRAZY_GAMES
            return await GetLangIdOnCrazyGames();
#else
            return LanguageId.Russian;
#endif
        }

        private async UniTask<LanguageId> GetLangIdOnCrazyGames()
        {
            LanguageId langId = LanguageId.English;
            bool isGet = false;

            CrazyGames.CrazySDK.Instance.GetSystemInfo(
                systemInfo =>
                {
                    isGet = true;
                    langId = systemInfo.countryCode.ToLower().AsLangId();
                });

            while (isGet == false)
                await UniTask.Yield();

            return langId;
        }

        private async UniTask<bool> IsMobileOnCrazyGames()
        {
            bool isMobile = false;
            bool isGet = false;

            CrazyGames.CrazySDK.Instance.GetSystemInfo(
                systemInfo =>
                {
                    isMobile = systemInfo.device.type is "mobile" or "tablet";
                    isGet = true;
                });

            while (isGet == false)
                await UniTask.Yield();

            return isMobile;
        }
    }
}