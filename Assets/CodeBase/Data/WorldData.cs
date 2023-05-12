using System;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public LevelState LevelState;
        public HealthState HeroHealthState;
        public StaminaState HeroStaminaState;
        public InventoryData HeroInventoryData;
        public ScoreAccumulationData ScoreAccumulationData;
        public string SceneName;

        public WorldData(string initialScene)
        {
            PositionOnLevel = new PositionOnLevel();
            LevelState = new LevelState(LevelNames.LobbyId);
            HeroHealthState = new HealthState();
            HeroStaminaState = new StaminaState();
            HeroInventoryData = new InventoryData();
            ScoreAccumulationData = new ScoreAccumulationData();
            SceneName = initialScene;
        }
    }
}