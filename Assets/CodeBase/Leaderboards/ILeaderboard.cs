using CodeBase.Data;
using Cysharp.Threading.Tasks;

namespace CodeBase.Leaderboards
{
    public interface ILeaderboard
    {
        UniTask<RanksData> GetRanksData();
        UniTask SetScore(int score, string avatarName);
        bool IsAuthorized { get; }
        UniTask<bool> TryAuthorize();
        UniTask<bool> TryRequestPersonalData();
    }
}