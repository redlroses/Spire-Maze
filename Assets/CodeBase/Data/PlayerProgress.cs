using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {

    public WorldData WorldData;

    public GlobalData GlobalData;

    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);

      GlobalData GlobalData = new GlobalData();
    }
  }
}