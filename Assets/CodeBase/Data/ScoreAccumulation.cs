﻿using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LevelAccumulationData
    {
        public float PlayTime;
        public int Artifacts;
        public int Score;
        public int Stars;
        public int Coins;
        public int TotalReviveTokens;

        public LevelAccumulationData()
        {
            PlayTime = 0f;
            Artifacts = 0;
            Score = 0;
            Stars = 0;
            Coins = 0;
            TotalReviveTokens = 1;
        }
    }
}