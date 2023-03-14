using CodeBase.Logic.HealthEntity;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public abstract class Trap : MonoCache
    {
        [SerializeField] protected TrapActivator Activator;

        private void Awake()
        {
            Construct(Activator);
            Debug.LogWarning("Remove constructor from awake");
        }

        public virtual void Construct(TrapActivator activator)
        {
            Activator = activator;
            Activator.Activated += Activate;
        }

        private void OnDestroy()
        {
            Activator.Activated -= Activate;
        }

        protected abstract void Activate(IDamagable damagable);
    }
}