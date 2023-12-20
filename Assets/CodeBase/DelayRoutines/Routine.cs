using System;

namespace CodeBase.DelayRoutines
{
    public abstract class Routine : IRoutine
    {
        private Action _executedCallback;

        protected Routine()
        {
            _executedCallback = () => { };
        }

        public bool IsActive { get; private set; }

        public void Play()
        {
            IsActive = true;
            OnPlay();
        }

        public void Stop()
        {
            if (this is Awaiter awaiter)
            {
                awaiter.Pause();

                return;
            }

            IsActive = false;
        }

        public void AddNext(IRoutine routine) =>
            _executedCallback = routine.Play;

        protected virtual void OnPlay()
        {
        }

        protected void Next()
        {
            IsActive = false;
            _executedCallback.Invoke();
        }
    }
}