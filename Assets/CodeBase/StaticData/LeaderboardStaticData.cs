using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LeaderboardSettings", menuName = "Static Data/Leaderboard")]
    public class LeaderboardStaticData : ScriptableObject
    {
        public string Name;
        public int TopPlayersCount;
        public int CompetingPlayersCount;
        public bool IsIncludeSelf;
    }
}