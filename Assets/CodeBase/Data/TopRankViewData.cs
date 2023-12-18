using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TopRankViewData
    {
        public Sprite Image;
        public Color32 TextColor;
    }
}