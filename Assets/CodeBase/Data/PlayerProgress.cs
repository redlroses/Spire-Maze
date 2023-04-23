using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public HealthState HeroHealthState;
    public StaminaState HeroStaminaState;
    public WorldData WorldData;
    public InventoryData HeroInventoryData;
    public ScoreAccumulationData ScoreAccumulationData;

    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      HeroHealthState = new HealthState();
      HeroStaminaState = new StaminaState();
      HeroInventoryData = new InventoryData();
      ScoreAccumulationData = new ScoreAccumulationData();
    }
  }
}