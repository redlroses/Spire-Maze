using System;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealth
    {
        event Action Died;
        int Points { get; }
        int MaxPoints { get; }
        bool IsAlive { get; }
    }
}