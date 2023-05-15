using System;

namespace CodeBase.Data
{
    [Serializable]
    public class ScoreAccumulationData
    {
        public float PlayTime;
        public int Artifacts;
        public int LevelScore;
        public int LevelStars;

        public void Reset()
        {
            PlayTime = 0f;
            Artifacts = 0;
            LevelScore = 0;
            LevelStars = 0;
        }
    }
}