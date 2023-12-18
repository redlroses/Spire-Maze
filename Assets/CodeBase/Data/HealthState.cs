using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HealthState
    {
        public int CurrentHP;
        public int MaxHP;

        public HealthState(int maxHp)
        {
            MaxHP = maxHp;
            CurrentHP = maxHp;
        }

        public HealthState()
        {
        }

        public void ResetHp() =>
            CurrentHP = MaxHP;
    }
}