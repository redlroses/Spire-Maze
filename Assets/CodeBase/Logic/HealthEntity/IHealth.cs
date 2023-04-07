using System;

namespace CodeBase.Logic.HealthEntity
{
    public interface IHealth : IDamagable, IParameter
    {
        event Action Died;
        bool IsAlive { get; }
    }
}