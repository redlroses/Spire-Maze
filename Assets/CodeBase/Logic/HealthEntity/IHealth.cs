using System;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealth : IDamagable
    {
        event Action Died;
        event Action Changed;
        int CurrentPoints { get; }
        int MaxPoints { get; }
        bool IsAlive { get; }
    }
}