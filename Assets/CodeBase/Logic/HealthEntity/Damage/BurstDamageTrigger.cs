using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity.Damage
{
    [RequireComponent(typeof(DamagableObserver))]
    public sealed class BurstDamageTrigger : ObserverTarget<DamagableObserver, IDamagable>, IDamageTrigger
    {
        [SerializeField] private int _damage;
        [SerializeField] private Collider _collider;

        private void Awake() =>
            _collider ??= GetComponent<Collider>();

        protected override void OnTriggerObserverEntered(IDamagable damagable) =>
            damagable.Damage(_damage, DamageType.Burst);

        public void Enable() =>
            _collider.enabled = true;

        public void Disable() =>
            _collider.enabled = false;
    }
}