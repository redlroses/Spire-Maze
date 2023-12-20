using System;
using NTC.Global.Cache;

namespace CodeBase.DelayRoutines
{
    // Unfinished, better not to use it
    public class EventAwaiter : Awaiter
    {
        private Action _callback;

        public EventAwaiter(
            Action callback,
            GlobalUpdate globalUpdate,
            Action<GlobalUpdate, Awaiter> addUpdater,
            Action<GlobalUpdate, Awaiter> removeUpdater)
            : base(globalUpdate, addUpdater, removeUpdater)
        {
            _callback = callback;
            Deactivate();
            _callback += Next;
        }

        protected override void OnUpdate(float deltaTime)
        {
        }
    }
}