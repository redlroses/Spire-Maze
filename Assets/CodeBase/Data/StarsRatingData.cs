using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class StarsRatingData
    {
        [Header("Score for")]
        [Range(0, 1000)] public int OneStar;
        [Range(0, 1000)] public int TwoStars;
        [Range(0, 1000)] public int ThreeStars;
    }
}