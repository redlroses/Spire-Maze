using CodeBase.StaticData;
using CodeBase.StaticData.Storable;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    LevelStaticData ForLevel(int levelId);
    HealthStaticData HealthForEntity(string entityKey);
    StaminaStaticData StaminaForEntity(string entityKey);
    StorableStaticData ForStorable(StorableType storableType);
    LeaderboardStaticData ForLeaderboard(string name);
    WindowConfig ForWindow(WindowId windowId);
    ScoreConfig ScoreForLevel(int levelId);
    Sprite SpriteByLang(string lang);
    Sprite GetDefaultAvatar();
  }
}