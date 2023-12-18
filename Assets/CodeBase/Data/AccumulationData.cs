using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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