using System;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.HealthEntity.Damage;
using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(TimerOperator))]
    [RequireComponent(typeof(DamagableObserver))]
    public class TrapActivator : ObserverTarget<DamagableObserver, IDamagable>
    {
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _activationDelay = 0.7f;
        [SerializeField] private float _recoveryTime;

        private bool _isActivated;

        public event Action Activated = () => { };

        private void Awake()
        {
            _timer ??= GetComponent<TimerOperator>();
        }

        private void WaitActivationDelay()
        {
            _timer.SetUp(_activationDelay, OnDelayPassed);
            _timer.Play();
        }

        private void WaitRecoveryTime()
        {
            _timer.SetUp(_recoveryTime, OnReadyToActivate);
            _timer.Play();
        }

        private void OnReadyToActivate()
        {
            _isActivated = false;
        }

        private void OnDelayPassed()
        {
            Activate();
        }

        protected override void OnTriggerObserverEntered(IDamagable _)
        {
            if (_isActivated)
            {
                return;
            }

            WaitActivationDelay();
        }

        private void Activate()
        {
            _isActivated = true;
            Activated.Invoke();
            WaitRecoveryTime();
        }
    }
}