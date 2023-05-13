using System;

namespace CodeBase.Data
{
    [Serializable]
    public class StaminaState
    {
        public float LowerSpeedMultiplier;
        public int CurrentValue;
        public int MaxValue;
        public float SpeedReplenish;
        public float DelayBeforeReplenish;

        public void ResetStamina()
        {
            CurrentValue = MaxValue;
        }
    }
}