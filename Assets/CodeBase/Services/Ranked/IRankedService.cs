using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Services.Ranked
{
    public interface IRankedService : IService
    {
        Task<RanksData> GetRanksData();
        void SetScore(int score, string avatarName = "Test1");
    }
}