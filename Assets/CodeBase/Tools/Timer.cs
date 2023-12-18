using System;
using NTC.Global.Cache.Interfaces;
using UnityEngine;

namespace CodeBase.Tools
{
    public class Timer : IRunSystem
    {
        private readonly float _duration;
        private readonly Action _onTimeIsOn;

        private float _elapsedTime;

        public Timer(float duration, Action onTimeIsOn)
        {
            _duration = duration;
            _elapsedTime = duration;
            _onTimeIsOn = onTimeIsOn;
        }

        public bool IsActive => _elapsedTime > 0;

        public void OnRun() =>
            Tick(Time.deltaTime);

        public void Tick(float deltaTime)
        {
            if (IsActive == false)
                return;

            _elapsedTime -= deltaTime;

            if (_elapsedTime <= 0)
                _onTimeIsOn?.Invoke();
        }

        public void Reset() =>
            _elapsedTime = _duration;
    }
}