using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Ranked;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IRankedService _rankedService;

        private Transform _uiRoot;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData,
            IPersistentProgressService progressService, IRankedService rankedService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _rankedService = rankedService;
        }

        public GameObject CreateExtraLiveView(Transform inside) =>
            _assets.Instantiate(AssetPath.ExtraLiveView, inside);

        public GameObject CreateTopRankView(int rank, Transform inside) =>
            _assets.Instantiate(AssetPath.TopRankView, inside);

        public GameObject CreateRankView(Transform inside)
        {
            return _assets.Instantiate(AssetPath.RankView, inside);
        }

        public void CreateSettings()
        {
            var window = CreateWindow<SettingsWindow>(WindowId.Settings);
            window.Construct(_progressService);
        }

        public void CreatePause()
        {
            var window = CreateWindow<PauseWindow>(WindowId.Pause);
            window.Construct(_progressService);
        }

        public void CreateResults()
        {
            var window = CreateWindow<ResultsWindow>(WindowId.Results);
            window.Construct(_progressService);
        }

        public void CreateLose()
        {
            var window = CreateWindow<LoseWindow>(WindowId.Lose);
            window.Construct(_progressService);
        }

        public void CreateLeaderboard()
        {
            var window = CreateWindow<LeaderboardWindow>(WindowId.Leaderboard);
            window.Construct(_rankedService, this);
        }

        public void CreateUIRoot()
        {
            GameObject root = _assets.Instantiate(AssetPath.UIRootPath);
            _uiRoot = root.transform;
        }

        private TWindow CreateWindow<TWindow>(WindowId windowId) where TWindow : WindowBase
        {
            WindowConfig config = _staticData.ForWindow(windowId);
            TWindow window = Object.Instantiate(config.Template, _uiRoot) as TWindow;
            return window;
        }
    }
}