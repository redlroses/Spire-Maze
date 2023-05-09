using CodeBase.Logic.HealthEntity;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public abstract class Trap : MonoCache
    {
        [SerializeField] protected TrapActivator Activator;

        protected int Id { get; private set; }

        public TrapActivator TrapActivator => Activator;

        public virtual void Construct(int id, TrapActivator activator)
        {
            Id = id;
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