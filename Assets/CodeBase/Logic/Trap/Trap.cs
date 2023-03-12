using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public abstract class Trap : MonoCache
    {
        [SerializeField] protected TrapActivator Activator;
        [SerializeField] protected int Damage;

        protected override void OnEnabled()
        {
            Activator.IsActived += Activate;
        }

        protected override void OnDisabled()
        {
            Activator.IsActived -= Activate;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamagable target) == false)
            {
                return;
            }

            target.ReceiveDamage(Damage);
        }

        protected abstract void Activate();
    }
}