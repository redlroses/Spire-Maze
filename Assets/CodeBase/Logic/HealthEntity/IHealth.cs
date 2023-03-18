using System;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealth : IDamagable
    {
        event Action Died;
        int CurrentPoints { get; }
        int MaxPoints { get; }
        bool IsAlive { get; }
    }
}