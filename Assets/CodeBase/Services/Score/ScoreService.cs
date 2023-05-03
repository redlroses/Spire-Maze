using System.Linq;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Services.Score
{
    public class ScoreService : IScoreService, ISavedProgress
    {
        private readonly IStaticDataService _staticData;

        private PlayerProgress _progress;
        private LevelData _currentLevelData;
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
            CurrentScore = _progress.WorldData.ScoreAccumulationData.Artifacts * scoreConfig.PerArtifact; //+ (scoreConfig.BasePointsOnStart - N * scoreConfig.PerSecondReduction);

            //Сохранять флаг что уровень пройден GlobalData.Levels.IsCompleted;
           // _currentLevelData.IsCompleted = true;
            return CurrentScore;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _progress = progress;
            _currentLevelId = _progress.WorldData.LevelState.LevelId;
            _currentLevelData = _progress.GlobalData.Levels.Find(level => level.Id == _currentLevelId);
            Debug.Log($"Level ID: {_currentLevelId}, LevelData: {_currentLevelData}");
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            _progress.WorldData.ScoreAccumulationData.Artifacts = _progress.WorldData.HeroInventoryData.InventoryCells
                .Where(inventoryCell => inventoryCell.Item.ItemType != StorableType.Key)
                .Sum(inventoryCell => inventoryCell.Count);
            //Сохранять текущий счет GlobalData.Levels.Score;
            _currentLevelData.Score = CurrentScore;
        }
    }
}