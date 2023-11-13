using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Services.Randomizer
{
    public class RandomModels : MonoCache
    {
        [SerializeField] private GameObject[] _models;

        private IRandomService _randomService;

        public void Initialize(IRandomService randomService, int seed)
        {
            _randomService = randomService;
            Randomize(seed);
        }

        private void Randomize(int seed)
        {
            for (int i = 0; i < _models.Length; i++)
            {
                int index = _randomService.RangeWithSeeding(0, _models.Length, seed+i);
                _models[index].SetActive(false);
            }
        }
    }
}