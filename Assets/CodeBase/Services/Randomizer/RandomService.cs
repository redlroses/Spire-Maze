using UnityEngine;

namespace CodeBase.Services.Randomizer
{
    public class RandomService : IRandomService
    {
        public float Range(float min, float max) =>
            Random.Range(min, max);

        public bool TryPass(float chance) =>
            Random.value <= chance;
    }
}