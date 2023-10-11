using System;

namespace CodeBase.DelayRoutines
{
    public class LoopWhile : Loop
    {
        private readonly Func<bool> _repeatCondition;
        private bool _repeatCondition1;

        public LoopWhile(Func<bool> repeatCondition)
        {
            _repeatCondition = repeatCondition;
        }

        protected override void OnPlay()
        {
            if (_repeatCondition.Invoke())
            {
                Iterate();
                return;
            }

            Next();
        }
    }
}