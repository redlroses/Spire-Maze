using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LevelData
    {
        public int Id;
        public int Score;
        public int Stars;

        public LevelData(int id, int score, int stars)
        {
            Id = id;
            Score = score;
            Stars = stars;
        }
    }
}