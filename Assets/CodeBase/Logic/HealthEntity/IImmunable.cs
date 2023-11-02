namespace CodeBase.Logic.HealthEntity
{
    public interface IImmunable : IHealthReactive
    {
        bool IsImmune { get; set; }
    }
}