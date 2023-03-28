using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "HealthData", menuName = "Static Data/Health")]
    public class HealthStaticData : ScriptableObject
    {
        public string EntityKey;
        [Range(0, 300)] public int MaxHealth = 100;
    }
}