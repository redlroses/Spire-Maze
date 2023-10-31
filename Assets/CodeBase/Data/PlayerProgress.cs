using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public GlobalData GlobalData;

        public int Relevance => GlobalData.Levels.Count;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            GlobalData = new GlobalData();
        }
    }
}