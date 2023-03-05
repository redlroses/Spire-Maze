using System;
using CodeBase.Tools;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class TimerOperator : MonoCache
    {
        private Timer _timer;
        private Action _callBack;

        private void Awake()
        {
            enabled = false;
        }

        protected override void Run()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            _timer.Tick(Time.deltaTime);
        }

        public void SetUp(float duration, Action action)
        {
            _timer = new Timer(duration, OnTimeIsOn);
            _callBack = action;
        }

        public void Play()
        {
            enabled = true;
        }

        public void Pause()
        {
            enabled = false;
        }

        public void Restart()
        {
            _timer.Reset();
        }

        private void OnTimeIsOn()
        {
            enabled = false;
            _callBack?.Invoke();
        }
    }
}