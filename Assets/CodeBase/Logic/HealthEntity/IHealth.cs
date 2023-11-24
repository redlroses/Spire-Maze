using System;
using CodeBase.Logic.HealthEntity.Damage;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealth : IDamagable, IPoints
    {
        event Action Died;
        bool IsAlive { get; }
    }
}