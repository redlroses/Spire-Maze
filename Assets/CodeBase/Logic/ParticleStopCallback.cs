using System;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleStopCallback : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;

        private Action _callback = () => { };

        private void Awake()
        {
            ParticleSystem.MainModule main = _particle.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void SetCallback(Action action) =>
            _callback = action;

        private void OnParticleSystemStopped()
        {
            _callback.Invoke();
        }
    }
}