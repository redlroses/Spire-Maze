using System;
using System.Collections;
using CodeBase.Infrastructure;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.Watch
{
    public class WatchService : IWatchService, IPauseWatcher
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPersistentProgressService _progressService;
        private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

        private Coroutine _routine;
        private float _elapsedTime;
        private bool _isPause;

        public event Action<float> TimeChanged;

        public WatchService(ICoroutineRunner coroutineRunnerRunner, IPersistentProgressService progressService)
        {
            _coroutineRunner = coroutineRunnerRunner;
            _progressService = progressService;
        }

        public void Start()
        {
            _routine ??= _coroutineRunner.StartCoroutine(RunTimer());
        }

        public void Cleanup()
        {
            ResetWatch();
        }

        private void ResetWatch()
        {
            _coroutineRunner.StopCoroutine(_routine);
            _routine = null;
            _elapsedTime = 0;
        }

        public void LoadProgress()
        {
            _elapsedTime = _progressService.Progress.WorldData.ScoreAccumulationData.PlayTime;
        }

        public void UpdateProgress()
        {
            _progressService.Progress.WorldData.ScoreAccumulationData.PlayTime = _elapsedTime;
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

        public void Resume()
        {
            _isPause = false;
        }

        public void Pause()
        {
            _isPause = true;
        }
    }
}