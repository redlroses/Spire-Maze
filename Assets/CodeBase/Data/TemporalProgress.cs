using UnityEngine;

namespace CodeBase.Data
{
    public class TemporalProgress
    {
        public int CoinsCount;
        public int CollectedArtifactsCount;
        public Vector2 LevelHeightRange;
        public int PlayTime;
        public int Score;
        public int StarsCount;
        public int TotalArtifactsCount;

        public TemporalProgress()
        {
            LevelHeightRange = Vector2.zero;
            PlayTime = 0;
            CollectedArtifactsCount = 0;
            TotalArtifactsCount = 0;
            Score = 0;
            StarsCount = 0;
            CoinsCount = 0;
        }
    }
}