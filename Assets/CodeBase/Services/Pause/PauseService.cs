using System;
using CodeBase.Services.Watch;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Services.Pause
{
    public class PauseService : IPauseService
    {
        private readonly IWindowService _windowService;

        public PauseService(IWatchService watchService)
        {
            watchService.RegisterPauseWatcher(this);
        }

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