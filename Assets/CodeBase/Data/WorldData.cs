using System;

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
        public AccumulationData AccumulationData;
        public string SceneName;

        public WorldData(string initialScene, int initialLevelId)
        {
            LevelPositions = new LevelPositions();
            LevelState = new LevelState(initialLevelId);
            HeroHealthState = new HealthState();
            HeroStaminaState = new StaminaState();
            HeroInventoryData = new InventoryData();
            AccumulationData = new AccumulationData();
            SceneName = initialScene;
        }
    }
}