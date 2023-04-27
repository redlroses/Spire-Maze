using System;
using CodeBase.Infrastructure.States;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public LevelState LevelState;
        public string LevelName;

        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel);
            LevelState = new LevelState(initialLevel);
            LevelName = initialLevel;
        }
    }
}