using System;
using CodeBase.Infrastructure.States;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public LevelState LevelState;
        public string SceneName;

        public WorldData(string initialScene)
        {
            PositionOnLevel = new PositionOnLevel();
            LevelState = new LevelState(LevelNames.LobbyId);
            SceneName = initialScene;
        }
    }
}