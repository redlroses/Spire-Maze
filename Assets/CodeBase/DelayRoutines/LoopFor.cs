namespace CodeBase.DelayRoutines
{
    public class LoopFor : Loop
    {
        private int _remainingTimes;

        public LoopFor(int times)
        {
            _remainingTimes = times;
        }

        protected override void OnPlay()
        {
            _remainingTimes--;

            if (_remainingTimes > 0)
            {
                Iterate();
                return;
            }

            Next();
        }
    }
}