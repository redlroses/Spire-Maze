using CodeBase.Logic.Player;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void Init(HeroReviver hero);
        void CreateLeaderboard();
        void CreateUIRoot();
        GameObject CreateExtraLiveView(Transform inside);
        GameObject CreateTopRankView(int rank, Transform inside);
        GameObject CreateRankView(Transform inside);
        GameObject CreateCellView(Transform selfTransform);
        GameObject CreateCompassArrowPanel(Transform hero, float lifetime);
        GameObject CreateOverviewInterface();
        GameObject CreateEnterLevelPanel();
        void CreateSettings();
        void CreatePause();
        void CreateResults();
        void CreateLose();
        GameObject CreateEditorRewardADPanel();
        GameObject CreateEditorInterstitialADPanel();
    }
}