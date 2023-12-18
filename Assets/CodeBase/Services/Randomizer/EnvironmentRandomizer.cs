using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Services.Randomizer
{
    public class EnvironmentRandomizer : MonoCache
    {
        [SerializeField] private SerializedDictionary<GameObject, float> _modelChances;

        private IRandomService _randomService;

        public void Construct(IRandomService randomService)
        {
            _randomService = randomService;
            EnableRandom();
        }

        public void EnableRandom()
        {
            KeyValuePair<GameObject, float>[] pairs = _modelChances.ToArray();

            for (int i = 0; i < pairs.Length; i++)
            {
                bool isPass = _randomService.TryPass(pairs[i].Value);
                pairs[i].Key.SetActive(isPass);
            }
        }
    }
}