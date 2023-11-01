using Random = UnityEngine.Random;

namespace CodeBase.Services.Randomizer
{
    public class RandomService : IRandomService
    {
        public int Range(int min, int max) =>
            Random.Range(min, max);

        public float Range(float min, float max) =>
            Random.Range(min, max);
    }
}