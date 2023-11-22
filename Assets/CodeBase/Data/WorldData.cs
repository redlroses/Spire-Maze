using System;
using CodeBase.Infrastructure;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public LevelPositions LevelPositions;
        public LevelState LevelState;
        public HealthState HeroHealthState;
        public StaminaState HeroStaminaState;
        public InventoryData HeroInventoryData;
        public LevelAccumulationData LevelAccumulationData;
        public string SceneName;

        public WorldData(string initialScene, int initialLevelId)
        {
            LevelPositions = new LevelPositions();
            LevelState = new LevelState(initialLevelId);
            HeroHealthState = new HealthState();
            HeroStaminaState = new StaminaState();
            HeroInventoryData = new InventoryData();
            LevelAccumulationData = new LevelAccumulationData();
            SceneName = initialScene;
        }
    }
}