using System;

namespace CodeBase.Logic.Observer
{
    public interface ITriggerObserverExit<out TTarget> : ITriggerObserver<TTarget>
    {
        event Action<TTarget> Exited;
    }
}