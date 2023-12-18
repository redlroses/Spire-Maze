using UnityEngine;

namespace CodeBase.Data
{
    public class SingleRankData
    {
        public readonly Sprite Avatar;
        public readonly Sprite Flag;
        public readonly string Name;
        public readonly int Rank;
        public readonly int Score;

        public SingleRankData(int rank, int score, Sprite avatar, string name, Sprite flag)
        {
            Rank = rank;
            Score = score;
            Avatar = avatar;
            Name = name;
            Flag = flag;
        }
    }
}