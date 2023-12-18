using CodeBase.StaticData;
using CodeBase.StaticData.Storable;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();

        LevelStaticData GetLevel(int levelId);

        HealthStaticData GetHealthEntity(string entityKey);

        StaminaStaticData GetStaminaEntity(string entityKey);

        StorableStaticData GetStorable(StorableType storableType);

        LeaderboardStaticData GetLeaderboard(string name);

        GameObject GetWindow(WindowId windowId);

        ScoreConfig GetScoreForLevel(int levelId);

        Sprite GetSpriteByLang(string lang);

        Sprite GetDefaultAvatar();

        CameraConfig GetCameraConfigByOrientation(string orientation);

        TutorialConfig GetTutorialConfig();

        bool HasLevel(int levelId);
    }
}