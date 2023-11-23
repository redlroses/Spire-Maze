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
    LevelStaticData GetForLevel(int levelId);
    HealthStaticData GetHealthForEntity(string entityKey);
    StaminaStaticData GetStaminaForEntity(string entityKey);
    StorableStaticData GetForStorable(StorableType storableType);
    LeaderboardStaticData GetForLeaderboard(string name);
    WindowConfig GetForWindow(WindowId windowId);
    ScoreConfig GetScoreForLevel(int levelId);
    Sprite GetSpriteByLang(string lang);
    Sprite GetDefaultAvatar();
    CameraConfig GetCameraConfigByOrientation(string orientation);
    TutorialConfig GetTutorialConfig();
  }
}