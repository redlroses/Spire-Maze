﻿using System;
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
        private const string ScoreConfigPath = "StaticData/WindowConfig/ScoreConfigs";

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<string, HealthStaticData> _healths;
        private Dictionary<string, StaminaStaticData> _staminas;
        private Dictionary<StorableType, StorableStaticData> _storables;
        private Dictionary<string, LeaderboardStaticData> _leaderboards;
        private Dictionary<WindowId, WindowConfig> _windows;
        private Dictionary<string, ScoreConfig> _scoreConfigs;

        public void Load()
        {
            _levels = LoadFor<LevelStaticData, string>(LevelsDataPath, x => x.LevelKey);
            _healths = LoadFor<HealthStaticData, string>(HealthPath, x => x.EntityKey);
            _staminas = LoadFor<StaminaStaticData, string>(StaminaPath, x => x.EntityKey);
            _storables = LoadFor<StorableStaticData, StorableType>(StorablePath, x => x.ItemType);
            _leaderboards = LoadFor<LeaderboardStaticData, string>(LeaderboardPath, x => x.Name);
            _scoreConfigs = LoadFor<ScoreConfig, string>(LeaderboardPath, x => x.LevelKey);
            _windows = Resources
                .Load<WindowStaticData>(WindowPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);
        }

        public LevelStaticData ForLevel(string levelKey) =>
            GetDataFor(levelKey, _levels);

        public HealthStaticData HealthForEntity(string entityKey) =>
            GetDataFor(entityKey, _healths);
        public StaminaStaticData StaminaForEntity(string entityKey) =>
            GetDataFor(entityKey, _staminas);

        public StorableStaticData ForStorable(StorableType storableType) =>
            GetDataFor(storableType, _storables);

        public LeaderboardStaticData ForLeaderboard(string yandexName) =>
            GetDataFor(yandexName, _leaderboards);

        public WindowConfig ForWindow(WindowId windowId) =>
            GetDataFor(windowId, _windows);

        public ScoreConfig ScoreForLevel(string levelKey) =>
            GetDataFor(levelKey, _scoreConfigs);

        private TData GetDataFor<TData, TKey>(TKey key, Dictionary<TKey, TData> from) =>
            from.TryGetValue(key, out TData staticData)
                ? staticData
                : throw new NullReferenceException($"There is no {nameof(TData)} data with key: {nameof(TKey)}");

        private Dictionary<TKey, TData> LoadFor<TData, TKey>(string path, Func<TData, TKey> keySelector) where TData : ScriptableObject =>
            Resources
                .LoadAll<TData>(path)
                .ToDictionary(keySelector, x => x);
    }
}