using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Leaderboard
{
    public interface ILeaderBoard
    {
        Task<RanksData[]> GetLeaderboardEntries();
        void SetScore(int score, string avatarName);
    }
}