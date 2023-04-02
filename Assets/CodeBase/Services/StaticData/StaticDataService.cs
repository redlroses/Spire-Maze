using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelsDataPath = "StaticData/Level Maps";
        private const string HealthPath = "StaticData/Health";
        private const string StorablePath = "StaticData/Storable";

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<string, HealthStaticData> _healths;
        private Dictionary<StorableType, StorableData> _storables;

        public void Load()
        {
            _levels = LoadFor<LevelStaticData, string>(LevelsDataPath, x => x.LevelKey);
            _healths = LoadFor<HealthStaticData, string>(HealthPath, x => x.EntityKey);
            _storables = LoadFor<StorableData, StorableType>(StorablePath, x => x.ItemType);
        }

        public LevelStaticData ForLevel(string levelKey) =>
            GetDataFor(levelKey, _levels);

        public HealthStaticData HealthForEntity(string entityKey) =>
            GetDataFor(entityKey, _healths);

        public StorableData ForStorable(StorableType storableType) =>
            GetDataFor(storableType, _storables);

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