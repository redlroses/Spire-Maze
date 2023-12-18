using System;
using CodeBase.Services.Pause;
using CodeBase.Tools;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class TimerOperator : MonoCache, IPauseWatcher
    {
        private Action _callBack;
        private bool _isEnabled;
        private Timer _timer;

        private void Awake() =>
            enabled = false;

        public void Resume() =>
            enabled = _isEnabled;

        public void Pause()
        {
            _isEnabled = enabled;
            enabled = false;
        }

        public void SetUp(float duration, Action action)
        {
            _timer = new Timer(duration, OnTimeIsOn);
            _callBack = action;
        }

        public void Play() =>
            enabled = true;

        public void Restart() =>
            _timer.Reset();

        protected override void Run() =>
            _timer.Tick(Time.deltaTime);

        private void OnTimeIsOn()
        {
            enabled = false;
            _callBack?.Invoke();
        }
    }
}