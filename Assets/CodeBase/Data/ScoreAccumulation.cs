using System;

namespace CodeBase.Data
{
    [Serializable]
    public class AccumulationData
    {
        public float PlayTime;
        public int TotalReviveTokens;

        public AccumulationData()
        {
            PlayTime = 0f;
            TotalReviveTokens = 1;
        }
    }
}