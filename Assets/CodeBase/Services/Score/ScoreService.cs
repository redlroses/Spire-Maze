using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;

namespace CodeBase.Services.Score
{
    public class ScoreService : IScoreService, ISavedProgress
    {
        private readonly IStaticDataService _staticData;

        private PlayerProgress _progress;
        private Level _currentLevel;
        private int _currentLevelId;

        public ScoreService(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public int CurrentScore { get; private set; } = default;

        public int CalculateScore()
        {
            // _progressService.Progress.ScoreAccumulationData;
            ScoreConfig scoreConfig = _staticData.ScoreForLevel(_currentLevelId);
            //TODO: Подсчёт очков по модификаторам из статик даты
            // CurrentScore = _progress.WorldData.ScoreAccumulationData.Artifacts * scoreConfig.PerArtifact + (scoreConfig.BasePointsOnStart - N * scoreConfig.PerSecondReduction);

            //Сохранять флаг что уровень пройден GlobalData.Levels.IsCompleted;
            _currentLevel.IsCompleted = true;
            return CurrentScore;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _progress = progress;
            _currentLevelId = progress.WorldData.LevelState.LevelId;
            _currentLevel = progress.GlobalData.Level.Find(level => level.Id == _currentLevelId);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            //Сохранять текущий счет GlobalData.Levels.Score;
            _currentLevel.Score = CurrentScore;
        }
    }
}