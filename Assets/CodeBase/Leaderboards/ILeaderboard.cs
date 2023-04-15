using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Leaderboard
{
    public interface ILeaderboard
    {
        Task<RanksData> GetRanksData();
        void SetScore(int score, string avatarName);
    }
}