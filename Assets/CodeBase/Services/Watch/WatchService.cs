using System;
using System.Collections;
using CodeBase.Infrastructure;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.Watch
{
    public class WatchService : IWatchService
    {
        private readonly ICoroutineRunner _coroutine;
        private readonly IPersistentProgressService _progressService;
        private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

        private float _elapsedTime;
        private IPauseReactive _pauseReactive;
        private bool _isPause;

        public event Action<float> TimeChanged;

        public WatchService(ICoroutineRunner coroutineRunner, IPersistentProgressService progressService)
        {
            _coroutine = coroutineRunner;
            _progressService = progressService;
        }

        public void Start()
        {
            _coroutine.StartCoroutine(RunTimer());
        }

        public void Cleanup()
        {
            _pauseReactive.Resume -= OnResume;
            _pauseReactive.Pause -= OnPause;
        }

        public void LoadProgress()
        {
            _elapsedTime = _progressService.Progress.WorldData.ScoreAccumulationData.PlayTime;
        }

        public void UpdateProgress()
        {
            _progressService.Progress.WorldData.ScoreAccumulationData.PlayTime = _elapsedTime;
        }

        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Resume += OnResume;
            _pauseReactive.Pause += OnPause;
        }

        private IEnumerator RunTimer()
        {
            while (true)
            {
                yield return _waitForSeconds;

                if (_isPause)
                {
                    continue;
                }

                _elapsedTime++;
                TimeChanged?.Invoke(_elapsedTime);
                Debug.Log($"Current elapsedTime {_elapsedTime}");
            }
        }

        private void OnResume()
        {
            _isPause = false;
        }

        private void OnPause()
        {
            _isPause = true;
        }
    }
}