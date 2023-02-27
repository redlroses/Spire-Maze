using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelsDataPath = "Level Maps";

        private Dictionary<string, LevelStaticData> _levels;

        public void Load()
        {
            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsDataPath)
                .ToDictionary(x => x.LevelKey, x => x);
        }

        public LevelStaticData ForLevel(string levelKey) =>
            _levels.TryGetValue(levelKey, out LevelStaticData staticData)
                ? staticData
                : throw new NullReferenceException($"There is no level with key: {levelKey}");
    }
}