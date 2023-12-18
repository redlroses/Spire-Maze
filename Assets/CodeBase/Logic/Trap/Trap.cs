using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public abstract class Trap : MonoCache
    {
        [SerializeField] private TrapActivator _activator;

        public TrapActivator TrapActivator => _activator;

        public void Construct(TrapActivator activator)
        {
            _activator = activator;
            _activator.Activated += Activate;
        }

        private void OnDestroy()
        {
            _activator.Activated -= Activate;
        }

        protected abstract void Activate();
    }
}