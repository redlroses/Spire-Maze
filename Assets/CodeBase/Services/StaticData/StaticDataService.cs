using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Storable;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelsDataPath = "StaticData/Level Maps";
        private const string HealthPath = "StaticData/Health";
        private const string StaminaPath = "StaticData/Stamina";
        private const string StorablePath = "StaticData/Storable";
        private const string LeaderboardPath = "StaticData/Leaderboard";
        private const string WindowPath = "StaticData/WindowConfig/WindowConfigs";
        private const string ScoreConfigPath = "StaticData/ScoreConfig";
        private const string FlagsConfigPath = "StaticData/FlagsConfig";
        private const string DefaultAvatarPath = "StaticData/DefaultAvatar";
        private const string CameraConfigsPath = "StaticData/CameraConfigs";
        private const string TutorialConfigPath = "StaticData/TutorialConfig";

        private Dictionary<int, LevelStaticData> _levels;
        private Dictionary<string, HealthStaticData> _healths;
        private Dictionary<string, StaminaStaticData> _staminas;
        private Dictionary<StorableType, StorableStaticData> _storables;
        private Dictionary<string, LeaderboardStaticData> _leaderboards;
        private Dictionary<WindowId, WindowConfig> _windows;
        private Dictionary<int, ScoreConfig> _scoreConfigs;
        private Dictionary<string, CameraConfig> _cameraConfigs;

        private FlagsConfig _flagsConfig;
        private TutorialConfig _tutorialConfig;


        public void Load()
        {
            _levels = LoadFor<LevelStaticData, int>(LevelsDataPath, x => x.LevelId);
            _healths = LoadFor<HealthStaticData, string>(HealthPath, x => x.EntityKey);
            _staminas = LoadFor<StaminaStaticData, string>(StaminaPath, x => x.EntityKey);
            _storables = LoadFor<StorableStaticData, StorableType>(StorablePath, x => x.ItemType);
            _leaderboards = LoadFor<LeaderboardStaticData, string>(LeaderboardPath, x => x.name);
            _scoreConfigs = LoadFor<ScoreConfig, int>(ScoreConfigPath, x => x.LevelId);
            _scoreConfigs = LoadFor<ScoreConfig, int>(ScoreConfigPath, x => x.LevelId);
            _cameraConfigs = LoadFor<CameraConfig, string>(CameraConfigsPath, x => x.Orientation.ToString());
            _windows = Resources
                .Load<WindowStaticData>(WindowPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);

            _flagsConfig = Resources.Load<FlagsConfig>(FlagsConfigPath);
            _tutorialConfig = Resources.Load<TutorialConfig>(TutorialConfigPath);
        }

        public LevelStaticData GetForLevel(int levelId) =>
            GetDataFor(levelId, _levels);

        public HealthStaticData GetHealthForEntity(string entityKey) =>
            GetDataFor(entityKey, _healths);

        public StaminaStaticData GetStaminaForEntity(string entityKey) =>
            GetDataFor(entityKey, _staminas);

        public StorableStaticData GetForStorable(StorableType storableType) =>
            GetDataFor(storableType, _storables);

        public LeaderboardStaticData GetForLeaderboard(string name) =>
            GetDataFor(name, _leaderboards);

        public WindowConfig GetForWindow(WindowId windowId) =>
            GetDataFor(windowId, _windows);

        public ScoreConfig GetScoreForLevel(int levelId) =>
            GetDataFor(levelId, _scoreConfigs);

        public Sprite GetSpriteByLang(string lang) =>
            _flagsConfig.Flags.TryGetValue(lang, out Sprite flag)
                ? flag
                : _flagsConfig.DefaultFlag;

        public Sprite GetDefaultAvatar() =>
            Resources.Load<Sprite>(DefaultAvatarPath);

        public CameraConfig GetCameraConfigByOrientation(string orientation) =>
            _cameraConfigs[orientation];

        public TutorialConfig GetTutorialConfig() =>
            _tutorialConfig;

        private TData GetDataFor<TData, TKey>(TKey key, IReadOnlyDictionary<TKey, TData> from) =>
            from.TryGetValue(key, out TData staticData)
                ? staticData
                : throw new NullReferenceException($"There is no {from.First().Value.GetType().Name} data with key: {key}");

        private Dictionary<TKey, TData> LoadFor<TData, TKey>(string path, Func<TData, TKey> keySelector) where TData : ScriptableObject =>
            Resources
                .LoadAll<TData>(path)
                .ToDictionary(keySelector, x => x);
    }
}