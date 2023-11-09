using System;
using System.Collections.Generic;
using CodeBase.Data.CellStates;

namespace CodeBase.Data
{
    [Serializable]
    public class LevelState
    {
        public int LevelId;
        public List<IndexableState> Indexables;

        public LevelState(int levelId)
        {
            LevelId = levelId;
            Indexables = new List<IndexableState>();
        }
    }
}