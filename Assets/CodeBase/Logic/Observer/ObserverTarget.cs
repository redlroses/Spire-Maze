using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Observer
{
    public abstract class ObserverTarget<TObserver, TTarget> : MonoCache where TObserver : ITriggerObserver<TTarget>
    {
        [SerializeField] private TObserver _observer;

        protected virtual void OnValidate() =>
            _observer ??= GetNearby<TObserver>();

        private void Awake()
        {
            _observer ??= GetNearby<TObserver>();
            OnAwake();
        }

        protected abstract void OnTriggerObserverEntered(TTarget target);

        protected virtual void OnAwake()
        {
        }

        protected override void OnEnabled() =>
            _observer.Entered += OnTriggerObserverEntered;

        protected override void OnDisabled() =>
            _observer.Entered -= OnTriggerObserverEntered;
    }
}