using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Observer
{
    public abstract class ObserverTargetExited<TObserver, TTarget> : MonoCache where TObserver : ITriggerObserverExit<TTarget>
    {
        [SerializeField] private TObserver _observer;

        private void Awake()
        {
            _observer ??= GetComponent<TObserver>();
        }

        protected override void OnEnabled()
        {
            _observer.Entered += OnTriggerObserverEntered;
            _observer.Exited += OnTriggerObserverExited;
        }

        protected override void OnDisabled()
        {
            _observer.Entered -= OnTriggerObserverEntered;
            _observer.Exited -= OnTriggerObserverExited;
        }

        protected abstract void OnTriggerObserverEntered(TTarget collectible);
        protected abstract void OnTriggerObserverExited(TTarget collectible);
    }
}