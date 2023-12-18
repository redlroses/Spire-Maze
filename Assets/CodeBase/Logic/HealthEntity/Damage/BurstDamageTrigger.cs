using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity.Damage
{
    [RequireComponent(typeof(DamagableObserver))]
    public sealed class BurstDamageTrigger : ObserverTarget<DamagableObserver, IDamagable>, IDamageTrigger
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private int _damage;
        [SerializeField] private bool _isLethal;

        private void Awake() =>
            _collider ??= GetComponent<Collider>();

        protected override void OnValidate() =>
            _collider ??= GetComponent<Collider>();

        public void Enable() =>
            _collider.enabled = true;

        public void Disable() =>
            _collider.enabled = false;

        protected override void OnTriggerObserverEntered(IDamagable damagable) =>
            damagable.Damage(_damage, _isLethal ? DamageType.Lethal : DamageType.Burst);
    }
}