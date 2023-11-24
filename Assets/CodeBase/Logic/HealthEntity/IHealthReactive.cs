using System;
using CodeBase.Logic.HealthEntity.Damage;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealthReactive : IHealth
    {
        event Action<int, DamageType> Damaged;
        event Action<int> Healed;
    }
}