using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "ScoreConfig", menuName = "Static Data/Score configs")]
    public class ScoreConfig : ScriptableObject
    {
        public string LevelKey;
        public int BasePointsOnStart;
        public int PerSecondReduction;
        public int PerArtifact;
        
        [Space] [Header("Stars Per Score")]
        public int[] StarsRatingData;
    }
}