using System.Linq;
using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Logic;
using CodeBase.Logic.Inventory;
using CodeBase.Services.Analytics;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Ranked;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Score;
using CodeBase.Services.StaticData;
using CodeBase.Services.Watch;
using CodeBase.StaticData;
using CodeBase.StaticData.Storable;
using CodeBase.UI.Services.Windows;
using CrazyGames;

namespace CodeBase.Infrastructure.States
{
    public class FinishState : IPayloadedState<bool>
    {
        private const int MaxStarsCount = 3;

        private readonly IAnalyticsService _analytics;
        private readonly IHeroLocator _heroLocator;
        private readonly IPersistentProgressService _progressService;
        private readonly IRankedService _rankedService;
        private readonly ISaveLoadService _saveLoad;
        private readonly IScoreService _scoreService;
        private readonly IStaticDataService _staticData;
        private readonly IWatchService _watchService;
        private readonly IWindowService _windowService;

        public FinishState(
            IAnalyticsService analytics,
            IHeroLocator heroLocator,
            IPersistentProgressService progressService,
            IRankedService rankedService,
            ISaveLoadService saveLoad,
            IScoreService scoreService,
            IStaticDataService staticData,
            IWatchService watchService,
            IWindowService windowService)
        {
            _windowService = windowService;
            _scoreService = scoreService;
            _rankedService = rankedService;
            _progressService = progressService;
            _saveLoad = saveLoad;
            _watchService = watchService;
            _heroLocator = heroLocator;
            _staticData = staticData;
            _analytics = analytics;
        }

        private TemporalProgress TemporalProgress => _progressService.TemporalProgress;

        private int LevelId => _progressService.Progress.WorldData.LevelState.LevelId;

        private int StarsCount => TemporalProgress.StarsCount;

        private int Score => TemporalProgress.Score;

        private int CollectedArtifactsCount => TemporalProgress.CollectedArtifactsCount;

        private int PlayTime => TemporalProgress.PlayTime;

        public void Enter(bool islose)
        {
            CountCollectedArtifacts();
            CountTotalArtifacts();
            CountPlayTime();
            CountLevelScore(islose);
            CountGlobalScore();

            if (islose)
            {
                _windowService.Open(WindowId.Loss);
            }
            else
            {
                _saveLoad.SaveProgress();
                _windowService.Open(WindowId.Results);

#if CRAZY_GAMES
                if (StarsCount >= MaxStarsCount)
                    CrazyEvents.Instance.HappyTime();
#endif
            }

            CollectAnalytics(islose);
        }

        public void Exit()
        {
        }

        private void CountCollectedArtifacts()
        {
            IInventory inventory = _heroLocator.Location.GetComponentInChildren<HeroInventory>().Inventory;
            int artifactsCount = inventory.Where(cell => cell.Item.IsArtifact).Sum(cell => cell.Count);
            _progressService.TemporalProgress.CollectedArtifactsCount = artifactsCount;
        }

        private void CountTotalArtifacts()
        {
            LevelStaticData levelData = _staticData.GetLevel(_progressService.Progress.WorldData.LevelState.LevelId);
            int artifactsCount = levelData.CellDataMap.OfType<ItemSpawnPoint>().Count(item => IsArtifact(item.Type));
            _progressService.TemporalProgress.TotalArtifactsCount = artifactsCount;
        }

        private void CountPlayTime() =>
            _progressService.TemporalProgress.PlayTime = _watchService.ElapsedSeconds;

        private void CountLevelScore(bool isLose) =>
            _scoreService.Calculate(isLose);

        private void CountGlobalScore()
        {
            int fullScore = _progressService.Progress.GlobalData.Levels.Sum(level => level.Score);
            _rankedService.SetScore(fullScore);
        }

        private bool IsArtifact(StorableType type) =>
            _staticData.GetStorable(type).IsArtifact;

        private void CollectAnalytics(bool isLose)
        {
            if (isLose)
                _analytics.TrackLevelLose(LevelId, StarsCount, PlayTime, CollectedArtifactsCount, Score);
            else
                _analytics.TrackLevelComplete(LevelId, StarsCount, PlayTime, CollectedArtifactsCount, Score);
        }
    }
}