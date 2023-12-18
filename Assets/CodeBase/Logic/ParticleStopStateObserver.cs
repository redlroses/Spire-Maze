using System;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleStopStateObserver : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;

        private Action _callback = () => { };

        private void Awake()
        {
            ParticleSystem.MainModule main = _particle.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped() =>
            _callback.Invoke();

        public void SetCallback(Action action) =>
            _callback = action;
    }
}