using System;

namespace CodeBase.Data
{
    [Serializable]
    public class StaminaState
    {
        public int CurrentValue;
        public int MaxValue;

        public void ResetStamina()
        {
            CurrentValue = MaxValue;
        }
    }
}