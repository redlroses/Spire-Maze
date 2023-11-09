using System;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealth : IDamagable, IPoints
    {
        event Action Died;
        bool IsAlive { get; }
    }
}