using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Services.Ranked
{
    public interface IRankedService : IService
    {
        bool IsAuthorized { get; }
        Task<bool> Authorize();
        Task<RanksData> GetRanksData();
        void SetScore(int score, string avatarName = "Test1");
        Task<bool> RequestPersonalData();
        void InitLeaderboard();
    }
}