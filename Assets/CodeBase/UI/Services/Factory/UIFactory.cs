using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.Hero;
using CodeBase.Services.ADS;
using CodeBase.Services.Localization;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Ranked;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Sound;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IRankedService _rankedService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ILocalizationService _localizationService;
        private readonly ISoundService _soundService;
        private readonly IPauseService _pauseService;
        private readonly IADService _adService;
        private readonly GameStateMachine _stateMachine;

        private Transform _uiRoot;
        private HeroReviver _hero;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData,
            IPersistentProgressService progressService, IRankedService rankedService, ISaveLoadService saveLoadService,
            ILocalizationService localizationService, ISoundService soundService, IPauseService pauseService, IADService adService, GameStateMachine stateMachine)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _rankedService = rankedService;
            _saveLoadService = saveLoadService;
            _localizationService = localizationService;
            _soundService = soundService;
            _pauseService = pauseService;
            _adService = adService;
            _stateMachine = stateMachine;
        }

        public void Init(HeroReviver hero) =>
            _hero = hero;

        public GameObject CreateEditorRewardADPanel() =>
            _assets.Instantiate(AssetPath.EditorRewardADPanel, _uiRoot);

        public GameObject CreateEditorInterstitialADPanel() =>
            _assets.Instantiate(AssetPath.EditorInterstitialADPanel, _uiRoot);

        public GameObject CreateExtraLiveView(Transform inside) =>
            _assets.Instantiate(AssetPath.ExtraLiveView, inside);

        public GameObject CreateTopRankView(int rank, Transform inside) =>
            _assets.Instantiate(AssetPath.TopRankView, inside);

        public GameObject CreateRankView(Transform inside) =>
            _assets.Instantiate(AssetPath.RankView, inside);

        public GameObject CreateCellView(Transform inside) =>
            InstantiateRegistered(AssetPath.CellView, inside);

        public GameObject CreateTutorialSequence() =>
            _assets.Instantiate(AssetPath.TutorialPanel, _uiRoot);

        public GameObject CreateCompassArrowPanel(Transform hero) =>
            _assets.Instantiate(AssetPath.CompassArrowPanel);

        public GameObject CreateEnterLevelPanel() =>
            _assets.Instantiate(AssetPath.EnterLevelPanel, _uiRoot);

        public void CreateSettings()
        {
            SettingsWindow window = CreateWindow<SettingsWindow>(WindowId.Settings);
            window.Construct(_localizationService, _soundService);
        }

        public void CreatePause()
        {
            PauseWindow window = CreateWindow<PauseWindow>(WindowId.Pause);
            window.Construct(_progressService, _pauseService, _stateMachine);
            _pauseService.Register(window);
        }

        public void CreateResults()
        {
            ResultsWindow window = CreateWindow<ResultsWindow>(WindowId.Results);
            window.Construct(_progressService, _stateMachine, _staticData);
        }

        public void CreateLose()
        {
            LoseWindow window = CreateWindow<LoseWindow>(WindowId.Lose);
            window.Construct(_progressService, _adService, _hero, _stateMachine);
        }

        public void CreateTutorial()
        {
            TutorialWindow window = CreateWindow<TutorialWindow>(WindowId.Tutorial);
            window.Construct(_staticData);
        }

        public GameObject CreateOverviewInterface() =>
            _assets.Instantiate(AssetPath.OverviewInterface, _uiRoot);

        public void CreateLeaderboard()
        {
            LeaderboardWindow window = CreateWindow<LeaderboardWindow>(WindowId.Leaderboard);
            window.Construct(_rankedService, this, _saveLoadService);
        }

        public void CreateUIRoot()
        {
            GameObject root = _assets.Instantiate(AssetPath.UIRoot);
            _uiRoot = root.transform;
            root.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        private TWindow CreateWindow<TWindow>(WindowId windowId) where TWindow : WindowBase
        {
            GameObject template = _staticData.GetWindow(windowId);
            TWindow window = Object.Instantiate(template, _uiRoot).GetComponent<TWindow>();
            return window;
        }

        private GameObject InstantiateRegistered(string path, Transform inside)
        {
            GameObject element = _assets.Instantiate(path, inside);
            Register(element);
            return element;
        }

        private void Register(GameObject element)
        {
            foreach (IPauseWatcher pauseWatcher in element.GetComponentsInChildren<IPauseWatcher>())
                _pauseService.Register(pauseWatcher);
        }
    }
}