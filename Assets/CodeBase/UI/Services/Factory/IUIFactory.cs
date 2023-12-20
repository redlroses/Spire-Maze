﻿using CodeBase.Logic.Hero;
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

        GameObject CreateTopRankView(Transform inside);

        GameObject CreateRankView(Transform inside);

        GameObject CreateCellView(Transform selfTransform);

        GameObject CreateCompassArrowPanel();

        GameObject CreateOverviewInterface();

        GameObject CreateEnterLevelPanel();

        GameObject CreateLevelNamePanel();

        GameObject CreateEditorRewardADPanel();

        GameObject CreateEditorInterstitialADPanel();

        GameObject CreateTutorialSequence();

        void CreateSettings();

        void CreatePause();

        void CreateResults();

        void CreateLose();

        void CreateTutorial();
    }
}