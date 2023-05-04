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
    }
}