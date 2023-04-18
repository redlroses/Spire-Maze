using CodeBase.StaticData;
using CodeBase.StaticData.Storable;

namespace CodeBase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    LevelStaticData ForLevel(string levelKey);
    HealthStaticData HealthForEntity(string entityKey);
    StaminaStaticData StaminaForEntity(string entityKey);
    StorableStaticData ForStorable(StorableType storableType);
    LeaderboardStaticData ForLeaderboard(string yandexName);
  }
}