using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class StaminaState
    {
        public int CurrentValue;
        public int MaxValue;

        public StaminaState(int maxStamina)
        {
            MaxValue = maxStamina;
            CurrentValue = maxStamina;
        }

        public StaminaState()
        {
        }

        public void Reset()
        {
            CurrentValue = MaxValue;
        }
    }
}