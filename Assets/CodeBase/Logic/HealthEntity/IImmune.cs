namespace CodeBase.Logic.HealthEntity
{
    public interface IImmune : IHealthReactive
    {
        bool IsImmune { get; }

        void ActivateImmunity();
        void DeactivateImmunity();
    }
}