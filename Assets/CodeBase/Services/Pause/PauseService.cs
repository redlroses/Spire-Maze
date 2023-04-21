using System;

namespace CodeBase.Services.Pause
{
    public class PauseService : IPauseService
    {
        public event Action Pause;
        public event Action Resume;

        public bool IsPause { get; private set; }

        public void SetPause(bool isPause)
        {
            if (IsPause == isPause)
            {
                return;
            }

            IsPause = isPause;

            if (IsPause)
            {
                Pause?.Invoke();
            }
            else
            {
                Resume?.Invoke();
            }
        }
    }
}