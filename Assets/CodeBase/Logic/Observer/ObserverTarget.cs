using UnityEngine;

namespace CodeBase.Logic.Observer
{
    public abstract class ObserverTarget<TObserver, TTarget> : MonoBehaviour where TObserver : ITriggerObserver<TTarget>
    {
        [SerializeField] private TObserver _observer;

        private void OnValidate() =>
            Awake();

        private void Awake() =>
            _observer ??= GetComponent<TObserver>();

        private void OnEnable() =>
            _observer.Entered += OnTriggerObserverEntered;

        private void OnDisable() =>
            _observer.Entered -= OnTriggerObserverEntered;

        protected abstract void OnTriggerObserverEntered(TTarget target);
    }
}