using System;
using CodeBase.Infrastructure.States;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public LevelState LevelState;

        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel);
            LevelState = new LevelState(LevelNames.LobbyKey);
        }
    }
}