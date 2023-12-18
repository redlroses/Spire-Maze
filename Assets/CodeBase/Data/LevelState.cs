using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CodeBase.Data.CellStates;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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