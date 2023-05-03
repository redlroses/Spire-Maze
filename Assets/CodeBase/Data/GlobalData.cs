using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class GlobalData
    {
        public List<LevelData> Levels = new List<LevelData>(MaxLevel);

        private const int MaxLevel = 15; 
        
        public GlobalData()
        {
            for (int i = 0; i < Levels.Capacity-1; i++)
            {
                Levels.Add(new LevelData()
                {
                    Id = i
                });
            }
        }
    }
}