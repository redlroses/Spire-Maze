namespace CodeBase.Services.Randomizer
{
  public interface IRandomService : IService
  {
    int Range(int minValue, int maxValue);
    float Range(float minValue, float maxValue);
    int RangeWithSeed(int min, int max, int seed);
    bool TryPass(float chance);
  }
}