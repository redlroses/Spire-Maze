using System;
using NTC.Global.Cache;

namespace CodeBase.DelayRoutines
{
    public abstract class TimeAwaiter : Awaiter
    {
        private float _elapsedTime;

        protected TimeAwaiter(GlobalUpdate globalUpdate, Action<GlobalUpdate, Awaiter> addUpdater,
            Action<GlobalUpdate, Awaiter> removeUpdater) : base(globalUpdate, addUpdater, removeUpdater) { }

        protected override void OnPlay()
        {
            _elapsedTime = GetWaitTime();
            base.OnPlay();
        }

        protected abstract float GetWaitTime();

        protected override void OnUpdate(float deltaTime)
        {
            _elapsedTime -= deltaTime;

            if (_elapsedTime <= 0)
            {
                Deactivate();
                Next();
            }
        }
    }
}