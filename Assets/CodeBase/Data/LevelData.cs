using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LevelData
    {
        public int Id;
        public int Score;

        public LevelData(int id, int score)
        {
            Id = id;
            Score = score;
        }
    }
}