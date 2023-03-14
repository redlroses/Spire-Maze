﻿using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(TimerOperator))]
    [RequireComponent(typeof(DamagableObserver))]
    public sealed class PeriodicDamageTrigger : ObserverTargetExited<DamagableObserver, IDamagable>, IDamageTrigger
    {
        [SerializeField] private float _tickPeriod;
        [SerializeField] private int _damage;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private Collider _collider;

        private IDamagable _damagable;

        private void Awake()
        {
            Construct();
            Debug.LogWarning("Remove constructor from awake");
        }

        public void Construct()
        {
            _timer.SetUp(_tickPeriod, OnDamage);
            _collider.isTrigger = true;
        }

        public void Enable()
        {
            _collider.enabled = true;
        }

        public void Disable()
        {
            _collider.enabled = false;
            _damagable = null;
            _timer.Pause();
        }

        private void OnDamage()
        {
            _damagable.ReceiveDamage(_damage);
            _timer.Restart();
            _timer.Play();
        }

        protected override void OnTriggerObserverEntered(IDamagable damagable)
        {
            _damagable = damagable;
            _timer.Restart();
            _timer.Play();
        }

        protected override void OnTriggerObserverExited(IDamagable damagable)
        {
            _timer.Pause();
            _damagable = null;
        }
    }
}