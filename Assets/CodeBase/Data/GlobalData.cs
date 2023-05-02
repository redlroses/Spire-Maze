using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class GlobalData
    {
        public List<LevelData> Levels = new List<LevelData>();

        public GlobalData()
        {
            
        }
    }
}