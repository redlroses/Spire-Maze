namespace CodeBase.Services.Randomizer
{
    public interface IRandomService : IService
    {
        float Range(float minValue, float maxValue);

        bool TryPass(float chance);
    }
}