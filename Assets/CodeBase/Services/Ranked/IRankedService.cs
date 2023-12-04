using CodeBase.Data;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.Ranked
{
    public interface IRankedService : IService
    {
        bool IsAuthorized { get; }
        UniTask<bool> Authorize();
        UniTask<RanksData> GetRanksData();
        void SetScore(int score, string avatarName = "Test1");
        UniTask<bool> RequestPersonalData();
        void InitLeaderboard();
    }
}