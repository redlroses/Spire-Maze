using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public GlobalData GlobalData;

        public PlayerProgress(string initialLevel, int initialLevelId)
        {
            WorldData = new WorldData(initialLevel, initialLevelId);
            GlobalData = new GlobalData();
        }

        public int Relevance => GlobalData.Levels.Count;
    }
}