﻿using CodeBase.Data;
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
        private const float ScoreToCoins = 0.35f;
        private const float LoseScoreFactor = 0.13f;

        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        private int _currentLevelId;
        private int _stars;
        private int _currentScore;
        private int _coins;

        private PlayerProgress Progress => _progressService.Progress;

        public ScoreService(IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _staticData = staticData;
        }

        public int Calculate(bool isLose)
        {
            LevelAccumulationData levelAccumulationData = Progress.WorldData.LevelAccumulationData;
            ScoreConfig scoreConfig = _staticData.GetScoreForLevel(_currentLevelId);

            if (isLose)
            {
                _currentScore = Mathf.RoundToInt(SumScore(levelAccumulationData, scoreConfig) * LoseScoreFactor);
                _stars = 0;
            }
            else
            {
                _currentScore = SumScore(levelAccumulationData, scoreConfig);
                _stars = StarsCountFromConfig(scoreConfig);
            }

            _coins = Mathf.RoundToInt(_currentScore * ScoreToCoins);

            if (isLose == false)
            {
                Progress.GlobalData.UpdateLevelData(_currentLevelId, _currentScore, _stars);
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

            Progress.WorldData.LevelAccumulationData.Score = _currentScore;
            Progress.WorldData.LevelAccumulationData.Stars = _stars;
            Progress.WorldData.LevelAccumulationData.Coins = _coins;
        }

        private void Cleanup()
        {
            _currentScore = 0;
            _stars = 0;
        }

        private int SumScore(LevelAccumulationData levelAccumulationData, ScoreConfig scoreConfig) =>
            (int) (ScorePerArtifacts(levelAccumulationData, scoreConfig) +
                   ScorePerTime(scoreConfig, levelAccumulationData));

        private float ScorePerTime(ScoreConfig scoreConfig, LevelAccumulationData levelAccumulationData)
        {
            int scorePerTime = scoreConfig.BasePointsOnStart -
                               (int) levelAccumulationData.PlayTime * scoreConfig.PerSecondReduction;
            return scorePerTime < 0 ? 0 : scorePerTime;
        }

        private int ScorePerArtifacts(LevelAccumulationData levelAccumulationData, ScoreConfig scoreConfig) =>
            levelAccumulationData.Artifacts *
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