using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity
{
    [RequireComponent(typeof(DamagableObserver))]
    public sealed class DamageTrigger : ObserverTarget<DamagableObserver, IDamagable>, IDamageTrigger
    {
        [SerializeField] private int _damage;
        [SerializeField] private Collider _collider;

        private void Awake()
        {
            Construct();
            Debug.LogWarning("Remove constructor from awake");
        }

        public void Construct()
        {
            _collider.isTrigger = true;
        }

        protected override void OnTriggerObserverEntered(IDamagable collectible)
        {
            collectible.Damage(_damage);
        }

        public void Enable()
        {
            _collider.enabled = true;
        }

        public void Disable()
        {
            _collider.enabled = false;
        }
    }
}