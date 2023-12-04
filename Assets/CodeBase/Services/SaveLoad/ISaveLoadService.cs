using CodeBase.Data;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.SaveLoad
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    UniTask<PlayerProgress> LoadProgress();
  }
}