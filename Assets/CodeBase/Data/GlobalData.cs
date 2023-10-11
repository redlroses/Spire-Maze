using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class GlobalData
    {
        public List<LevelData> Levels;

        public GlobalData()
        {
            Levels = new List<LevelData>();
        }

        public void UpdateLevelData(int levelId, int score, int stars)
        {
            LevelData levelData = Levels.Find(level => level.Id == levelId);

            if (levelData is null)
            {
                Levels.Add(new LevelData(levelId, score, stars));
            }
            else
            {
                if (levelData.Score < score)
                {
                    levelData.Score = score;
                    levelData.Stars = stars;
                }
            }
        }
    }
}