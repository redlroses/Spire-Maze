using System;
using NTC.Global.Cache;

namespace CodeBase.DelayRoutines
{
    public class UntilAwaiter : ActionAwaiter
    {
        public UntilAwaiter(Func<bool> waitFunc, GlobalUpdate globalUpdate, Action<GlobalUpdate, Awaiter> addUpdater,
            Action<GlobalUpdate, Awaiter> removeUpdater)
            : base(waitFunc, globalUpdate, addUpdater, removeUpdater) { }

        protected override bool IsAwaiting(Func<bool> waitFunc) =>
            !waitFunc.Invoke();
    }
}