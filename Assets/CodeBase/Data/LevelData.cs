using System;

namespace CodeBase.Data
{
    [Serializable]
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