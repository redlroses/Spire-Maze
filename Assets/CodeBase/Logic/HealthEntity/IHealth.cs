using System;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealth : IDamagable, IBar
    {
        event Action Died;
        bool IsAlive { get; }
    }
}