using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public abstract class Trap : MonoCache
    {
        [SerializeField] private TrapActivator _activator;

        public TrapActivator TrapActivator => _activator;

        private void OnDestroy()
        {
            _activator.Activated -= OnActivate;
        }

        public void Construct(TrapActivator activator)
        {
            _activator = activator;
            _activator.Activated += OnActivate;
        }

        protected abstract void OnActivate();
    }
}