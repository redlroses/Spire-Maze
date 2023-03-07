using System;

namespace CodeBase.Data
{
  [Serializable]
  public class HealthState
  {
    public float CurrentHP;
    public float MaxHP;

    public void ResetHP()
    {
      CurrentHP = MaxHP;
    }
  }
}