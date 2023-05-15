using System;
using System.Collections.Generic;
using CodeBase.Data.CellStates;

namespace CodeBase.Data
{
    [Serializable]
    public class LevelState
    {
        public int LevelId;
        public List<EnemyState> EnemyStates;
        public List<IndexableState> Indexables;

        public LevelState(int levelId)
        {
            LevelId = levelId;
            EnemyStates = new List<EnemyState>();
            Indexables = new List<IndexableState>();
        }
    }
}