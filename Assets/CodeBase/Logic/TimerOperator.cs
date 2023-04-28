using System;
using CodeBase.Services.Pause;
using CodeBase.Tools;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class TimerOperator : MonoCache, IPauseWatcher
    {
        private Timer _timer;
        private Action _callBack;
        private IPauseReactive _pauseReactive;
        private bool _isEnabled;

        private void Awake()
        {
            enabled = false;
        }

        private void OnDestroy()
        {
            _pauseReactive.Pause -= OnPause;
            _pauseReactive.Resume -= OnResume;
        }

        protected override void Run()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            _timer.Tick(Time.deltaTime);
        }

        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Pause += OnPause;
            _pauseReactive.Resume += OnResume;
        }

        public void SetUp(float duration, Action action)
        {
            _timer = new Timer(duration, OnTimeIsOn);
            _callBack = action;
        }

        public void Play() => enabled = true;

        public void Pause() => enabled = false;

        public void Restart() => _timer.Reset();

        private void OnTimeIsOn()
        {
            enabled = false;
            _callBack?.Invoke();
        }

        private void OnResume()
        {
            enabled = _isEnabled;
        }

        private void OnPause()
        {
            _isEnabled = enabled;
            enabled = false;
        }
    }
}