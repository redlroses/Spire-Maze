using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Services.SaveLoad
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    Task<PlayerProgress> LoadProgress();
  }
}