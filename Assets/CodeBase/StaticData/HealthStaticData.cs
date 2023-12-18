using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "HealthData", menuName = "Static Data/Health")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HealthStaticData : ScriptableObject
    {
        [SerializeField] public string EntityKey;
        [Range(0, 300)] public int MaxHealth = 100;
    }
}