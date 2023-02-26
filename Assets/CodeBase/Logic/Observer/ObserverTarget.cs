using UnityEngine;

namespace CodeBase.Logic.Observer
{
    public abstract class ObserverTarget<TObserver, TTarget> : MonoBehaviour where TObserver : TriggerObserver<TTarget>
    {
        [SerializeField] private TObserver _observer;

        private void Awake()
        {
            _observer ??= GetComponent<TObserver>();
        }

        private void OnEnable()
        {
            _observer.Entered += OnTriggerObserverEntered;
        }

        private void OnDisable()
        {
            _observer.Entered += OnTriggerObserverEntered;
        }

        protected abstract void OnTriggerObserverEntered(TTarget collectible);
    }
}