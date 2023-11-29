using System;
using CodeBase.DelayRoutines;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.Watch
{
    public class WatchService : IWatchService
    {
        private readonly IPersistentProgressService _progressService;
        private readonly RoutineSequence _watchRoutine;

        public WatchService(IPersistentProgressService progressService)
        {
            _progressService = progressService;

            _watchRoutine = new RoutineSequence()
                .WaitWhile(Tick)
                .Then(() => SecondTicked.Invoke(ElapsedSeconds))
                .LoopWhile(true);
        }

        public event Action<int> SecondTicked = _ => { };

        public float ElapsedTime { get; private set; }
        public int ElapsedSeconds { get; private set; }

        public void Start() =>
            _watchRoutine.Play();

        public void Cleanup() =>
            ResetWatch();

        public void LoadProgress()
        {
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
            _watchRoutine.Stop();
        }

        private bool Tick()
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