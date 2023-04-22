using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class PointsPerLevelProgress : ScriptableObject
    {
        public int BasePointsOnStart;
        public int PerSecondReduction;
        public int PerArtifact;
    }
}