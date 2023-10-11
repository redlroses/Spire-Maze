using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Services.Score
{
    public class ScoreService : IScoreService
    {
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        private int _currentLevelId;
        private int _stars;
        private int _currentScore;

        private PlayerProgress Progress => _progressService.Progress;

        public ScoreService(IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _staticData = staticData;
        }

        public int CalculateScore()
        {
            ScoreAccumulationData scoreAccumulationData = Progress.WorldData.ScoreAccumulationData;
            ScoreConfig scoreConfig = _staticData.ScoreForLevel(_currentLevelId);

            _currentScore = SumScore(scoreAccumulationData, scoreConfig);
            _stars = StarsCountFromConfig(scoreConfig);

            return _currentScore = _currentScore > 0 ? _currentScore : 0;
        }

        public void LoadProgress()
        {
            if (IsScorableLevel() == false)
            {
                return;
            }

            Cleanup();

            _currentLevelId = Progress.WorldData.LevelState.LevelId;

#if DEBUG
            LevelData currentLevelData = Progress.GlobalData.Levels.Find(level => level.Id == _currentLevelId);

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

            Progress.GlobalData.UpdateLevelData(_currentLevelId, _currentScore, _stars);

            Progress.WorldData.ScoreAccumulationData.LevelScore = _currentScore;
            Progress.WorldData.ScoreAccumulationData.LevelStars = _stars;
        }

        private void Cleanup()
        {
            _currentScore = 0;
            _stars = 0;
        }

        private int SumScore(ScoreAccumulationData scoreAccumulationData, ScoreConfig scoreConfig) =>
            (int) (ScorePerArtifacts(scoreAccumulationData, scoreConfig) +
                   ScorePerTime(scoreConfig, scoreAccumulationData));

        private float ScorePerTime(ScoreConfig scoreConfig, ScoreAccumulationData scoreAccumulationData)
        {
            int scorePerTime = scoreConfig.BasePointsOnStart -
                               (int) scoreAccumulationData.PlayTime * scoreConfig.PerSecondReduction;
            return scorePerTime < 0 ? 0 : scorePerTime;
        }

        private int ScorePerArtifacts(ScoreAccumulationData scoreAccumulationData, ScoreConfig scoreConfig) =>
            scoreAccumulationData.Artifacts *
            scoreConfig.PerArtifact;

        private bool IsScorableLevel() =>
            SceneManager.GetActiveScene().name == LevelNames.BuildableLevel;

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