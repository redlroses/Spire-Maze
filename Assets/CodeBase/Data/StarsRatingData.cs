using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class StarsRatingData
    {
        [Header("Score for")]
        [Range(0, 1000)] public int OneStar;
        [Range(0, 1000)] public int TwoStars;
        [Range(0, 1000)] public int ThreeStars;
    }
}