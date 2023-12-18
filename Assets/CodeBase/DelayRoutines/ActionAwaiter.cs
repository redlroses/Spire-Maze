using System;
using NTC.Global.Cache;

namespace CodeBase.DelayRoutines
{
    public abstract class ActionAwaiter : Awaiter
    {
        private readonly Func<bool> _waitFunc;

        protected ActionAwaiter(Func<bool> waitFunc,
            GlobalUpdate globalUpdate,
            Action<GlobalUpdate, Awaiter> addUpdater,
            Action<GlobalUpdate, Awaiter> removeUpdater)
            : base(globalUpdate, addUpdater, removeUpdater)
        {
            _waitFunc = waitFunc;
        }

        protected abstract bool IsAwaiting(Func<bool> waitFunc);

        protected override void OnUpdate(float deltaTime)
        {
            if (IsAwaiting(_waitFunc))
                return;

            Deactivate();
            Next();
        }
    }
}