using System;
using System.Collections;
using CodeBase.Infrastructure;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.Watch
{
    public class WatchService : IWatchService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPersistentProgressService _progressService;
        private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

        private Coroutine _routine;
        private float _elapsedTime;
        private bool _isPause;
        private bool _isRun;

        public event Action<float> TimeChanged = _ => { };

        public float ElapsedTime => _elapsedTime;

        public WatchService(ICoroutineRunner coroutineRunnerRunner, IPersistentProgressService progressService)
        {
            _coroutineRunner = coroutineRunnerRunner;
            _progressService = progressService;
        }

        public void Start()
        {
            Resume();
            _isRun = true;
            _routine ??= _coroutineRunner.StartCoroutine(RunTimer());
        }

        public void Cleanup() =>
            ResetWatch();

        public void LoadProgress()
        {
            _elapsedTime = _progressService.Progress.WorldData.AccumulationData.PlayTime;
            TimeChanged.Invoke(_elapsedTime);
        }

        public void UpdateProgress() =>
            _progressService.Progress.WorldData.AccumulationData.PlayTime = _elapsedTime;

        public void Resume() =>
            _isPause = false;

        public void Pause() =>
            _isPause = true;

        private void ResetWatch()
        {
            _isRun = false;
            _coroutineRunner.StopCoroutine(_routine);
            _routine = null;
        }

        private IEnumerator RunTimer()
        {
            while (_isRun)
            {
                yield return _waitForSeconds;

                if (_isPause)
                {
                    continue;
                }

                TimeChanged.Invoke(++_elapsedTime);
            }
        }
    }
}