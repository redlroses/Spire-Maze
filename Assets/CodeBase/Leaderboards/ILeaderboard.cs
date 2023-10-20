using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Leaderboards
{
    public interface ILeaderboard
    {
        Task<RanksData> GetRanksData();
        Task SetScore(int score, string avatarName);
    }
}