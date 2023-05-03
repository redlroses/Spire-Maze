using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {

    public WorldData WorldData;

    public GlobalData GlobalData;

    public PlayerProgress(string initialLevel, int countLevels)
    {
      WorldData = new WorldData(initialLevel);
      GlobalData = new GlobalData(countLevels);
    }
  }
}