using System;

namespace CodeBase.Data
{
    [Serializable]
    public class ScoreAccumulationData
    {
        public float PlayTime;
        public int Artifacts;

        public void Reset()
        {
            PlayTime = 0;
            Artifacts = 0;
        }
    }
}