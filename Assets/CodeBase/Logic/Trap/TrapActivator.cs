using System;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(TimerOperator))]
    [RequireComponent(typeof(DamagableObserver))]
    public class TrapActivator : ObserverTarget<DamagableObserver, IDamagable>
    {
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _recoveryTime;

        private bool _isActivated;

        public event Action<IDamagable> Activated;

        private void Awake()
        {
            Construct();
            Debug.LogWarning("Remove constructor from awake");
        }

        public void Construct()
        {
            _timer.SetUp(_recoveryTime, OnReadyToActivate);
        }

        private void OnReadyToActivate()
        {
            _isActivated = false;
        }

        protected override void OnTriggerObserverEntered(IDamagable collectible)
        {
            if (_isActivated)
            {
                return;
            }

            _timer.Restart();
            _timer.Play();
            _isActivated = true;
            Activated?.Invoke(collectible);
        }
    }
}