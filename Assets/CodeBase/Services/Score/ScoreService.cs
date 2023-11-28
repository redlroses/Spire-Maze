using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.StaticData.Storable;
using CodeBase.Tools.Extension;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Services.Score
{
    public class ScoreService : IScoreService
    {
        private const float ScoreToCoins = 0.35f;
        private const float LoseScoreFactor = 0.13f;

        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        private int _currentLevelId;
        private int _stars;
        private int _currentScore;
        private int _coins;

        private TemporalProgress TemporalProgress => _progressService.TemporalProgress;
        private GlobalData GlobalData => _progressService.Progress.GlobalData;
        private WorldData WorldData => _progressService.Progress.WorldData;
        private AccumulationData AccumulationData => _progressService.Progress.WorldData.AccumulationData;


        public ScoreService(IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _staticData = staticData;
        }

        public int Calculate(bool isLose)
        {
            ScoreConfig scoreConfig = _staticData.GetScoreForLevel(_currentLevelId);

            if (isLose)
            {
                _currentScore = Mathf.RoundToInt(SumScore(scoreConfig) * LoseScoreFactor);
                _stars = 0;
            }
            else
            {
                _currentScore = SumScore(scoreConfig);
                _stars = StarsCountFromConfig(scoreConfig);
            }

            _coins = Mathf.RoundToInt(_currentScore * ScoreToCoins);

            if (isLose == false)
            {
                GlobalData.UpdateLevelData(_currentLevelId, _currentScore, _stars);
            }

            return _currentScore;
        }

        public void LoadProgress()
        {
            if (IsScorableLevel() == false)
            {
                return;
            }

            Cleanup();

            _currentLevelId = WorldData.LevelState.LevelId;

#if DEBUG
            LevelData currentLevelData = GlobalData.Levels.Find(level => level.Id == _currentLevelId);

            Debug.Log(currentLevelData == null
                ? $"Level ID: {_currentLevelId}, LevelData: {currentLevelData}"
                : $"Level ID: {_currentLevelId}, LevelData: id - {currentLevelData.Id}, best score - {currentLevelData.Score}, stars - {currentLevelData.Stars}");
#endif
        }

        public void UpdateProgress()
        {
            if (IsScorableLevel() == false)
            {
                return;
            }

            TemporalProgress.Score = _currentScore;
            TemporalProgress.StarsCount = _stars;
            TemporalProgress.CoinsCount = _coins;
        }

        private void Cleanup()
        {
            _currentScore = 0;
            _stars = 0;
        }

        private int SumScore(ScoreConfig scoreConfig) =>
            ScorePerArtifacts(scoreConfig) +
            ScorePerTime(scoreConfig);

        private int ScorePerTime(ScoreConfig scoreConfig)
        {
            int scorePerTime = scoreConfig.BasePointsOnStart - (int) AccumulationData.PlayTime * scoreConfig.PerSecondReduction;
            return scorePerTime < 0 ? 0 : scorePerTime;
        }

        private int ScorePerArtifacts(ScoreConfig scoreConfig)
        {
            int artifactsCount = WorldData.HeroInventoryData.ItemsData.Count(item => ((StorableType) item.StorableType).IsArtifact());
            return artifactsCount * scoreConfig.PerArtifact;
        }

        private bool IsScorableLevel()
        {
            string name = SceneManager.GetActiveScene().name;
            return name.Equals(LevelNames.BuildableLevel) || name.Equals(LevelNames.LearningLevel);
        }

        private int StarsCountFromConfig(ScoreConfig scoreConfig)
        {
            for (int i = scoreConfig.StarsRatingData.Length - 1; i >= 0; i--)
            {
                if (_currentScore > scoreConfig.StarsRatingData[i])
                {
                    return i + 1;
                }
            }

            return 0;
        }
    }
}