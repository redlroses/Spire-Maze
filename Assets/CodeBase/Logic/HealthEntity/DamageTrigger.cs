using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity
{
    [RequireComponent(typeof(DamagableObserver))]
    public sealed class DamageTrigger : ObserverTarget<DamagableObserver, IDamagable>, IDamageTrigger
    {
        [SerializeField] private int _damage;
        [SerializeField] private Collider _collider;

        private void Awake() =>
            _collider ??= GetComponent<Collider>();

        protected override void OnTriggerObserverEntered(IDamagable damagable) =>
            damagable.Damage(_damage, DamageType.Single);

        public void Enable() =>
            _collider.enabled = true;

        public void Disable() =>
            _collider.enabled = false;
    }
}