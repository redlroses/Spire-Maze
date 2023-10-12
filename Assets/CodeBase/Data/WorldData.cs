using System;
using CodeBase.Infrastructure;
using CodeBase.Tools.Extension;
using UnityEngine;

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
        public LevelAccumulationData _levelAccumulationData;
        public string SceneName;

        public WorldData(string initialScene)
        {
            LevelPositions = new LevelPositions();
            LevelState = new LevelState(LevelNames.LobbyId);
            HeroHealthState = new HealthState();
            HeroStaminaState = new StaminaState();
            HeroInventoryData = new InventoryData();
            _levelAccumulationData = new LevelAccumulationData();
            SceneName = initialScene;
        }
    }
}