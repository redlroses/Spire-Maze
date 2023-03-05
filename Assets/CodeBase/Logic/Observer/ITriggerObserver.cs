using System;

namespace CodeBase.Logic.Observer
{
    public interface ITriggerObserver<out TTarget>
    {
        event Action<TTarget> Entered;
    }
}