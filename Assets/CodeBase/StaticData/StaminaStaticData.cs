using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "StaminaData", menuName = "Static Data/Stamina")]
    public class StaminaStaticData : ScriptableObject
    {
        public string EntityKey;
        [Range(0, 3000)] public int MaxStamina = 1000;
        public float LowerSpeedMultiplier = 0.5f;
        public float SpeedReplenish = 500;
        public float DelayBeforeReplenish = 1.5f;
    }
}