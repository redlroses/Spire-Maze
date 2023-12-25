using System.Collections.Generic;
using System.Linq;
#if !UNITY_EDITOR && UNITY_WEBGL
using Agava.WebUtility;
#endif
using AYellowpaper.SerializedCollections;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Services.Randomizer
{
    public class EnvironmentRandomizer : MonoCache
    {
        [SerializeField] private float _mobileChanceFactor = 0.5f;
        [SerializeField] private SerializedDictionary<GameObject, float> _modelChances;

        private IRandomService _randomService;

        public void Construct(IRandomService randomService)
        {
            _randomService = randomService;
            EnableRandom();
        }

        public void EnableRandom()
        {
            float chanceFactor = 1;

#if !UNITY_EDITOR && UNITY_WEBGL
            if (Device.IsMobile)
                chanceFactor = _mobileChanceFactor;
#endif

            KeyValuePair<GameObject, float>[] pairs = _modelChances.ToArray();

            for (int i = 0; i < pairs.Length; i++)
            {
                bool isPass = _randomService.TryPass(pairs[i].Value * chanceFactor);
                pairs[i].Key.SetActive(isPass);
            }
        }
    }
}