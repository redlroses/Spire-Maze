using CodeBase.StaticData;

namespace CodeBase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    LevelStaticData ForLevel(string levelKey);
    HealthStaticData HealthForEntity(string entityKey);
  }
}