using System;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealthReactive : IHealth
    {
        event Action<int, DamageType> Damaged;
        event Action<int> Healed;
    }
}