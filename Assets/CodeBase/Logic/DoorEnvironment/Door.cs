using CodeBase.Data.Cell;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Сollectible;
using UnityEngine;

namespace CodeBase.Logic.DoorEnvironment
{
    [RequireComponent(typeof(KeyCollectorObserver))]
    public class Door : ObserverTarget<KeyCollectorObserver, KeyCollector>
    {
        [SerializeField] private Colors _doorColor;
        [SerializeField] private DoorAnimator _animator;
        [SerializeField] private MaterialChanger _materialChanger;

        private Transform _selfTransform;

        private void Start()
        {
            _selfTransform = transform;
        }

        protected override void OnTriggerObserverEntered(KeyCollector collectible)
        {
            if (collectible.TryUseKey(_doorColor))
            {
                Open(collectible);
            }
        }

        public void Construct(IGameFactory gameFactory, Colors doorDataColor)
        {
            _materialChanger.Construct(gameFactory);
            _materialChanger.SetMaterial(doorDataColor);
            _doorColor = doorDataColor;
        }

        private void Open(KeyCollector keyCollector)
        {
            _animator.Open(GetDirection(keyCollector.transform.position));
        }

        private float GetDirection(Vector3 collectorPosition)
        {
            Vector3 directionToCollector = collectorPosition - _selfTransform.position;
            return Vector3.Dot(directionToCollector, _selfTransform.forward);
        }
    }
}