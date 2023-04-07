using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public HealthState HeroHealthState;
    public StaminaState HeroStaminaState;
    public WorldData WorldData;
    public HeroInventoryOld HeroInventoryOld;
    public InventoryData HeroInventoryData;

    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      HeroHealthState = new HealthState();
      HeroStaminaState = new StaminaState();
      HeroInventoryOld = new HeroInventoryOld();
      HeroInventoryData = new InventoryData();
    }
  }
}