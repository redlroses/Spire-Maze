using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity
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
            _timer ??= Get<TimerOperator>();
            _collider ??= Get<Collider>();
            _timer.SetUp(_tickPeriod, OnDamage);
        }

        public void Enable() =>
            _collider.enabled = true;

        public void Disable()
        {
            _collider.enabled = false;
            _damagable = null;
            _timer.Pause();
        }

        protected override void OnTriggerObserverEntered(IDamagable damagable)
        {
            _damagable = damagable;
            _timer.Restart();
            _timer.Play();
            OnDamage();
        }

        protected override void OnTriggerObserverExited(IDamagable damagable)
        {
            _timer.Pause();
            _damagable = null;
        }

        private void OnDamage()
        {
            _damagable.Damage(_damage, DamageType.Periodic);
            _timer.Restart();
            _timer.Play();
        }
    }
}