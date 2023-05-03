using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class GlobalData
    {
        public List<LevelData> Levels;

        public GlobalData(int countLevels)
        {
            Levels = new List<LevelData>(countLevels);

            for (int i = 0; i < Levels.Capacity; i++)
            {
                Levels.Add(new LevelData()
                {
                    Id = i
                });
            }
        }
    }
}