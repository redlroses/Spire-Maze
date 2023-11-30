﻿using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.Inventory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Ranked;
using CodeBase.Services.Score;
using CodeBase.StaticData.Storable;
using CodeBase.Tools.Extension;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.States
{
    public class FinishState : IPayloadedState<bool>
    {
        private readonly IWindowService _windowService;
        private readonly IScoreService _scoreService;
        private readonly IRankedService _rankedService;
        private readonly IPersistentProgressService _progressService;
        private readonly IHeroLocator _heroLocator;

        public FinishState(IWindowService windowService, IScoreService scoreService, IRankedService rankedService,
            IPersistentProgressService progressService, IHeroLocator heroLocator)
        {
            _progressService = progressService;
            _heroLocator = heroLocator;
            _rankedService = rankedService;
            _windowService = windowService;
            _scoreService = scoreService;
        }

        public void Enter(bool isLose)
        {
            CountArtifacts();
            CountLevelScore(isLose);
            CountGlobalScore();
            _windowService.Open(isLose ? WindowId.Lose : WindowId.Results);
        }

        public void Exit() { }

        private void CountLevelScore(bool isLose) =>
            _scoreService.Calculate(isLose);

        private void CountGlobalScore()
        {
            int fullScore = _progressService.Progress.GlobalData.Levels.Sum(level => level.Score);
            _rankedService.SetScore(fullScore);
        }

        private void CountArtifacts()
        {
            IInventory inventory = _heroLocator.Location.GetComponentInChildren<HeroInventory>().Inventory;
            int artifactsCount = inventory.Count(cell => cell.Item.IsArtifact);
            _progressService.TemporalProgress.ArtifactsCount = artifactsCount;
        }
    }
}