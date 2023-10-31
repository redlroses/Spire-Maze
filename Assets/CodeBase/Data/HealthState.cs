using System;

namespace CodeBase.Data
{
    [Serializable]
    public class HealthState
    {
        public int CurrentHP;
        public int MaxHP;

        public HealthState(int maxHp)
        {
            MaxHP = maxHp;
            CurrentHP = maxHp;
        }

        public HealthState() { }

        public void ResetHP()
        {
            CurrentHP = MaxHP;
        }
    }
}