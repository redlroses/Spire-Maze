using System;

namespace CodeBase.DelayRoutines
{
    public sealed class Executor : Routine
    {
        private readonly Action _action;

        public Executor(Action action) =>
            _action = action;

        protected override void OnPlay()
        {
            _action.Invoke();
            Next();
        }
    }
}