using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Services.Ranked
{
    public interface IRankedService : IService
    {
        Task<RanksData> GetRanksData();
        void SetScore(in int score, string avatarName);
    }
}