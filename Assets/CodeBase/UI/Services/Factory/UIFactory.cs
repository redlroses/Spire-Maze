﻿using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Player;
using CodeBase.Services.ADS;
using CodeBase.Services.Localization;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Ranked;
using CodeBase.Services.Sound;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.Tools.Extension;
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
        private readonly ILocalizationService _localizationService;
        private readonly ISoundService _soundService;
        private readonly IPauseService _pauseService;
        private readonly IADService _adService;
        private readonly GameStateMachine _stateMachine;

        private Transform _uiRoot;
        private HeroReviver _hero;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData,
            IPersistentProgressService progressService, IRankedService rankedService,
            ILocalizationService localizationService, ISoundService soundService, IPauseService pauseService, IADService adService, GameStateMachine stateMachine)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _rankedService = rankedService;
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

        public GameObject CreateCompassArrowPanel(Transform hero, float lifetime)
        {
            GameObject panel = _assets.Instantiate(AssetPath.CompassArrowPanel);
            CompassArrowPanel compassPanel = panel.GetComponent<CompassArrowPanel>();
            Canvas canvas = panel.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            compassPanel.Construct(_progressService.Progress.WorldData.LevelPositions.FinishPosition.AsUnityVector(), hero, lifetime);
            return panel;
        }

        public GameObject CreateEnterLevelPanel() =>
            _assets.Instantiate(AssetPath.EnterLevelPanel, _uiRoot);

        public void CreateSettings()
        {
            var window = CreateWindow<SettingsWindow>(WindowId.Settings);
            window.Construct(_localizationService, _soundService);
        }

        public void CreatePause()
        {
            var window = CreateWindow<PauseWindow>(WindowId.Pause);
            window.Construct(_progressService, _pauseService, _stateMachine);
            _pauseService.Register(window);
        }

        public void CreateResults()
        {
            var window = CreateWindow<ResultsWindow>(WindowId.Results);
            window.Construct(_progressService, _stateMachine);
        }

        public void CreateLose()
        {
            var window = CreateWindow<LoseWindow>(WindowId.Lose);
            window.Construct(_progressService, _adService, _hero, _stateMachine);
        }

        public GameObject CreateOverviewInterface() =>
            _assets.Instantiate(AssetPath.OverviewInterface, _uiRoot);

        public void CreateLeaderboard()
        {
            var window = CreateWindow<LeaderboardWindow>(WindowId.Leaderboard);
            window.Construct(_rankedService, this);
        }

        public void CreateUIRoot()
        {
            GameObject root = _assets.Instantiate(AssetPath.UIRootPath);
            _uiRoot = root.transform;
            root.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        private TWindow CreateWindow<TWindow>(WindowId windowId) where TWindow : WindowBase
        {
            WindowConfig config = _staticData.GetForWindow(windowId);
            TWindow window = Object.Instantiate(config.Template, _uiRoot) as TWindow;
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