using System.Collections.Generic;
using CodeBase.Services.Watch;
using CodeBase.Tools;

namespace CodeBase.Services.Pause
{
    public class PauseService : IPauseService
    {
        private readonly IWatchService _watchService;

        private List<IPauseWatcher> PauseWatchers { get; } = new List<IPauseWatcher>();
        private List<IPauseWatcher> UnregisteredPauseWatchers { get; } = new List<IPauseWatcher>();

        public PauseService(IWatchService watchService)
        {
            _watchService = watchService;
            WebFocusObserver.InBackgroundChangeEvent += OnInBackgroundChanged;
        }

        public bool IsPause { get; private set; }

        public void Register(IPauseWatcher pauseWatcher) =>
            PauseWatchers.Add(pauseWatcher);

        public void Unregister(IPauseWatcher pauseWatcher)
        {
            if (UnregisteredPauseWatchers.Contains(pauseWatcher))
                return;

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
            ValidateWatchers();

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

            for (int index = 0; index < PauseWatchers.Count; index++)
            {
                IPauseWatcher pauseWatcher = PauseWatchers[index];
                pauseWatcher.Resume();
            }
        }

        private void SendPause()
        {
            _watchService.Pause();

            for (int index = 0; index < PauseWatchers.Count; index++)
            {
                IPauseWatcher pauseWatcher = PauseWatchers[index];
                pauseWatcher.Pause();
            }
        }

        private void ValidateWatchers() =>
            PauseWatchers.RemoveAll(watcher => watcher.Equals(null));

        private void OnInBackgroundChanged(bool isHidden)
        {
            SetPause(isHidden);
        }
    }
}