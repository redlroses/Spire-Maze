using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Services.Randomizer
{
    public class EnvironmentRandomizer : MonoCache
    {
        [SerializeField] [Range(0f, 1f)] private float _environmentDensity;
        [SerializeField] private GameObject[] _models;

        private IRandomService _randomService;

        public void Construct(IRandomService randomService)
        {
            _randomService = randomService;
            EnableRandom();
        }

        public void EnableRandom()
        {
            for (int i = 0; i < _models.Length; i++)
            {
                bool isPass = _randomService.TryPass(_environmentDensity);
                _models[i].SetActive(isPass);
            }
        }
    }
}