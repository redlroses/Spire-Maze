namespace CodeBase.DelayRoutines
{
    public abstract class Loop : Routine
    {
        private IRoutine _loopStart;

        public void AddLoopStart(IRoutine routine) =>
            _loopStart = routine;

        protected void Iterate() =>
            _loopStart.Play();
    }
}