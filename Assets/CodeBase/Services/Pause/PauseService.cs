﻿using System.Collections.Generic;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Services.Pause
{
    public class PauseService : IPauseService
    {
        private Locker _cachedLocker;

        public PauseService()
        {
            WebFocusObserver.InBackgroundChangeEvent += OnInBackgroundChanged;
        }

        private List<IPauseWatcher> PauseWatchers { get; } = new List<IPauseWatcher>();

        private List<IPauseWatcher> UnregisteredPauseWatchers { get; } = new List<IPauseWatcher>();

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
            _cachedLocker = null;
            IsPause = false;
        }

        public void EnablePause(Locker locker = null)
        {
            if (IsPause || _cachedLocker is not null)
                return;

            IsPause = true;
            Debug.Log(IsPause);
            _cachedLocker = locker;
            ValidateWatchers();
            SendPause();
        }

        public void DisablePause(Locker locker = null)
        {
            if (IsPause == false || _cachedLocker != locker)
                return;

            IsPause = false;
            Debug.Log(IsPause);
            _cachedLocker = null;
            ValidateWatchers();
            SendResume();
        }

        private void SendResume()
        {
            for (int index = 0; index < PauseWatchers.Count; index++)
            {
                IPauseWatcher pauseWatcher = PauseWatchers[index];
                pauseWatcher.Resume();
            }
        }

        private void SendPause()
        {
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
            if (isHidden)
                EnablePause();
            else
                DisablePause();
        }
    }
}