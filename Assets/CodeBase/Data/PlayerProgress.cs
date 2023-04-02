using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public HealthState HeroHealthState;
    public WorldData WorldData;
    public HeroInventoryOld HeroInventoryOld;
    public InventoryData HeroInventoryData;

    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      HeroHealthState = new HealthState();
      HeroInventoryOld = new HeroInventoryOld();
      HeroInventoryData = new InventoryData();
    }
  }
}