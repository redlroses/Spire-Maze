using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "ScoreConfig", menuName = "Static Data/Score configs")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ScoreConfig : ScriptableObject
    {
        public string LevelKey;
        public int LevelId;
        public int BasePointsOnStart;
        public int PerSecondReduction;
        public int PerArtifact;

        [Space] [Header("Stars Per Score")]
        public int[] StarsRatingData;
    }
}