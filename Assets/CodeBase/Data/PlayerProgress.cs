using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public HealthState HeroHealthState;
    public WorldData WorldData;
    public HeroInventory HeroInventory;

    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      HeroHealthState = new HealthState();
      HeroInventory = new HeroInventory();
    }
  }
}