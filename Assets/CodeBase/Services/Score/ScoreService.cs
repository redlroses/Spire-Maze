using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;

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

        public ScoreService(IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _staticData = staticData;
        }

        private TemporalProgress TemporalProgress => _progressService.TemporalProgress;

        private GlobalData GlobalData => _progressService.Progress.GlobalData;

        private WorldData WorldData => _progressService.Progress.WorldData;

        public int Calculate(bool isLose)
        {
            Cleanup();

            _currentLevelId = WorldData.LevelState.LevelId;
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
                GlobalData.UpdateLevelData(_currentLevelId, _currentScore, _stars);

            UpdateProgress();

            return _currentScore;
        }

        private void UpdateProgress()
        {
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
            int scorePerTime = scoreConfig.BasePointsOnStart -
                               TemporalProgress.PlayTime * scoreConfig.PerSecondReduction;
            return scorePerTime < 0 ? 0 : scorePerTime;
        }

        private int ScorePerArtifacts(ScoreConfig scoreConfig) =>
            TemporalProgress.CollectedArtifactsCount * scoreConfig.PerArtifact;

        private int StarsCountFromConfig(ScoreConfig scoreConfig)
        {
            for (int i = scoreConfig.StarsRatingData.Length - 1; i >= 0; i--)
            {
                if (_currentScore > scoreConfig.StarsRatingData[i])
                    return i + 1;
            }

            return 0;
        }
    }
}