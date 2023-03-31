using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelsDataPath = "StaticData/Level Maps";
        private const string HealthPath = "StaticData/Health";

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<string, HealthStaticData> _healths;

        public void Load()
        {
            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsDataPath)
                .ToDictionary(x => x.LevelKey, x => x);

            _healths = Resources
                .LoadAll<HealthStaticData>(HealthPath)
                .ToDictionary(x => x.EntityKey, x => x);
        }

        public LevelStaticData ForLevel(string levelKey) =>
            _levels.TryGetValue(levelKey, out LevelStaticData staticData)
                ? staticData
                : throw new NullReferenceException($"There is no level with key: {levelKey}");

        public HealthStaticData HealthForEntity(string entityKey) =>
            _healths.TryGetValue(entityKey, out HealthStaticData staticData)
                ? staticData
                : throw new NullReferenceException($"There is no health data for: {entityKey}");
    }
}