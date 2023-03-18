using System;

namespace CodeBase.Data
{
  [Serializable]
  public class HealthState
  {
    public int CurrentHP;
    public int MaxHP;

    public void ResetHP()
    {
      CurrentHP = MaxHP;
    }
  }
}