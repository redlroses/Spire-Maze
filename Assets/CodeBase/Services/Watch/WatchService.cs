using System;
using CodeBase.DelayRoutines;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Services.Watch
{
    public class WatchService : IWatchService, IPauseWatcher
    {
        private readonly IPersistentProgressService _progressService;
        private readonly RoutineSequence _watchRoutine;

        public WatchService(IPersistentProgressService progressService)
        {
            _progressService = progressService;

            _watchRoutine = new RoutineSequence()
                .WaitWhile(SecondTick)
                .Then(() => SecondTicked.Invoke(ElapsedSeconds))
                .LoopWhile(true);
        }

        public event Action<int> SecondTicked = _ => { };

        public float ElapsedTime { get; private set; }
        public int ElapsedSeconds { get; private set; }

        public void Start() =>
            _watchRoutine.Play();

        public void Stop() =>
            _watchRoutine.Stop();

        public void Cleanup()
        {
            ResetWatch();
            Stop();
        }

        public void LoadProgress()
        {
            if (ElapsedTime.EqualsApproximately(0f))
                ElapsedTime = _progressService.Progress.WorldData.AccumulationData.PlayTime;

            ElapsedSeconds = GetSeconds(ElapsedTime);
            SecondTicked.Invoke(ElapsedSeconds);
        }

        public void UpdateProgress() =>
            _progressService.Progress.WorldData.AccumulationData.PlayTime = ElapsedTime;

        public void Resume() =>
            _watchRoutine.Play();

        public void Pause() =>
            _watchRoutine.Stop();

        private void ResetWatch()
        {
            ElapsedTime = 0;
            ElapsedSeconds = 0;
        }

        private bool SecondTick()
        {
            ElapsedTime += Time.deltaTime;

            int seconds = GetSeconds(ElapsedTime);

            if (seconds <= ElapsedSeconds)
                return false;

            ElapsedSeconds = seconds;
            return true;
        }

        private int GetSeconds(float time) =>
            Mathf.FloorToInt(time);
    }
}