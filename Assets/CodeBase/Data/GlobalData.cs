using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GlobalData
    {
        public List<LevelData> Levels;
        public SoundVolume SoundVolume;

        public GlobalData()
        {
            Levels = new List<LevelData>();
            SoundVolume = new SoundVolume();
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