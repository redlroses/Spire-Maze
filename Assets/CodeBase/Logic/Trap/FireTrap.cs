using System;
using CodeBase.Logic.HealthEntity;
using CodeBase.Services.Pause;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(TimerOperator))]
    public class FireTrap : Trap, IPauseWatcher
    {
        [SerializeField]
        [RequireInterface(typeof(IDamageTrigger))] private MonoBehaviour _damageTrigger;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _turnOffDelay;

        private bool _isActivated;

        private IDamageTrigger DamageTrigger => (IDamageTrigger)_damageTrigger;

        private void Awake() =>
            _timer.SetUp(_turnOffDelay, OnTurnOff);

        public void Resume()
        {
            if (_isActivated)
                _effect.Play();
        }

        public void Pause()
        {
            if (_isActivated)
                _effect.Pause();
        }

        private void OnTurnOff()
        {
            DamageTrigger.Disable();
            _effect.Stop();
            _isActivated = false;
        }

        protected override void Activate(IDamagable damagable)
        {
            if (_isActivated)
                return;

            _timer.Restart();
            _timer.Play();
            _effect.Play();
            DamageTrigger.Enable();
            _isActivated = true;
        }
    }
}