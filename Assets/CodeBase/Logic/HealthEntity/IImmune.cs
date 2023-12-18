namespace CodeBase.Logic.HealthEntity
{
    public interface IImmune
    {
        bool IsImmune { get; }

        void ActivateImmunity();

        void DeactivateImmunity();
    }
}