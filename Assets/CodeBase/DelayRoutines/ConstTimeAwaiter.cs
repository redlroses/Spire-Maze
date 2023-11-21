using System;
using NTC.Global.Cache;

namespace CodeBase.DelayRoutines
{
    public sealed class ConstTimeAwaiter : TimeAwaiter
    {
        private readonly float _waitTime;

        public ConstTimeAwaiter(float waitTime, GlobalUpdate globalUpdate, Action<GlobalUpdate, Awaiter> addUpdater,
            Action<GlobalUpdate, Awaiter> removeUpdater) : base(globalUpdate, addUpdater, removeUpdater) =>
            _waitTime = waitTime;

        protected override float GetWaitTime() =>
            _waitTime;
    }
}