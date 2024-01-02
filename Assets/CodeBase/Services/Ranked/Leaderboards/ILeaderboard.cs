using CodeBase.Data;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.Ranked.Leaderboards
{
    public interface ILeaderboard
    {
        bool IsAuthorized { get; }

        UniTask<RanksData> GetRanksData();

        UniTask SetScore(int score, string avatarName);

        UniTask<bool> TryAuthorize();

        UniTask<bool> TryRequestPersonalData();
    }
}