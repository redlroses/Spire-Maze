﻿using UnityEngine;

namespace CodeBase.Data
{
    public class TemporalProgress
    {
        public Vector2 LevelHeightRange;
        public int ArtifactsCount;
        public int PlayTime;
        public int Score;
        public int StarsCount;
        public int CoinsCount;


        public TemporalProgress()
        {
            LevelHeightRange = Vector2.zero;
            PlayTime = 0;
            ArtifactsCount = 0;
            Score = 0;
            StarsCount = 0;
            CoinsCount = 0;
        }
    }
}