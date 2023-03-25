using System;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleStopCallback : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;

        private Action _callback;

        private void Awake()
        {
            var main = _particle.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void SetCallback(Action action) => _callback = action;

        void OnParticleSystemStopped()
        {
            _callback?.Invoke();
        }
    }
}