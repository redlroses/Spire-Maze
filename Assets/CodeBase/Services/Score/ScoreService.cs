using CodeBase.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace CodeBase.Services.Score
{
    public class ScoreService : IScoreService
    {
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        private LevelData _currentLevelData;
        private int _currentLevelId;

        private PlayerProgress Progress => _progressService.Progress;

        public ScoreService(IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _staticData = staticData;
        }

        public int CurrentScore { get; private set; } = default;

        public int CalculateScore()
        {
            ScoreAccumulationData scoreAccumulationData = Progress.WorldData.ScoreAccumulationData;
            ScoreConfig scoreConfig = _staticData.ScoreForLevel(_currentLevelId);

            CurrentScore =
                (int) (scoreAccumulationData.Artifacts *
                    scoreConfig.PerArtifact + (scoreConfig.BasePointsOnStart -
                                               scoreAccumulationData.PlayTime * scoreConfig.PerSecondReduction));

            return CurrentScore = CurrentScore > 0 ? CurrentScore : 0;
        }

        public void LoadProgress()
        {
            if (IsScorableLevel() == false)
            {
                return;
            }

            _currentLevelId = Progress.WorldData.LevelState.LevelId;
            _currentLevelData = Progress.GlobalData.Levels.Find(level => level.Id == _currentLevelId);

            if (_currentLevelData != null)
            {
                CurrentScore = _currentLevelData.Score;
            }

            Debug.Log(_currentLevelData == null
                ? $"Level ID: {_currentLevelId}, LevelData: {_currentLevelData}"
                : $"Level ID: {_currentLevelId}, LevelData: id - {_currentLevelData.Id}, score - {_currentLevelData.Score}");
        }

        public void UpdateProgress()
        {
            if (_currentLevelData == null)
            {
                Progress.GlobalData.Levels.Add(new LevelData(_currentLevelId, CurrentScore));
            }
            else
            {
                if (_currentLevelData.Score < CurrentScore)
                {
                    _currentLevelData.Score = CurrentScore;
                }
            }
        }

        private bool IsScorableLevel() =>
            SceneManager.GetActiveScene().name == LevelNames.BuildableLevel;
    }
}