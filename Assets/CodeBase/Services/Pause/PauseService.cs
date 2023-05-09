using System.Collections.Generic;
using CodeBase.Services.Watch;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Services.Pause
{
    public class PauseService : IPauseService
    {
        private readonly IWatchService _watchService;
        private readonly IWindowService _windowService;

        public List<IPauseWatcher> PauseWatchers { get; } = new List<IPauseWatcher>();
        public List<IPauseWatcher> UnregisteredPauseWatchers { get; } = new List<IPauseWatcher>();

        public PauseService(IWatchService watchService)
        {
            _watchService = watchService;
        }

        public bool IsPause { get; private set; }

        public void Register(IPauseWatcher pauseWatcher) =>
            PauseWatchers.Add(pauseWatcher);

        public void UnregisterAll(IPauseWatcher pauseWatcher)
        {
            if (UnregisteredPauseWatchers.Contains(pauseWatcher) == false)
                UnregisteredPauseWatchers.Add(pauseWatcher);
        }

        public void Cleanup()
        {
            PauseWatchers.Clear();
            UnregisteredPauseWatchers.Clear();
        }

        public void SetPause(bool isPause)
        {
            if (IsPause == isPause)
            {
                return;
            }

            IsPause = isPause;

            if (isPause)
            {
                SendPause();
            }
            else
            {
                SendResume();
            }
        }

        private void SendResume()
        {
            _watchService.Resume();

            foreach (var pauseWatcher in PauseWatchers)
                pauseWatcher.Resume();

            UnregisterAll();
        }

        private void SendPause()
        {
            _watchService.Pause();

            foreach (var pauseWatcher in PauseWatchers)
                pauseWatcher.Pause();

            UnregisterAll();
        }

        private void UnregisterAll()
        {
            foreach (var unregisteredPauseWatcher in UnregisteredPauseWatchers)
            {
                if (PauseWatchers.Contains(unregisteredPauseWatcher))
                {
                    PauseWatchers.Remove(unregisteredPauseWatcher);
                }
            }
        }
    }
}