using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Logic.Inventory;
using CodeBase.Logic.Observer;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Storable;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.DoorEnvironment
{
    [RequireComponent(typeof(InventoryObserver))]
    public class Door : ObserverTarget<InventoryObserver, HeroInventory>, ISavedProgress, IIndexable
    {
        [SerializeField] private DoorAnimator _animator;

        private StorableType _targetKeyColor;
        private Transform _selfTransform;

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public void Construct(Colors doorColor, int id)
        {
            _targetKeyColor = doorColor.ToStorableType();
            _selfTransform = transform;
            Id = id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables.Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                return;
            }

            if (cellState.IsActivated)
            {
                _animator.Open(1f);
                IsActivated = true;
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables.Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.Indexables.Add(new IndexableState(Id, IsActivated));
            }
            else
            {
                cellState.IsActivated = IsActivated;
            }
        }

        protected override void OnTriggerObserverEntered(HeroInventory heroInventory)
        {
            if (heroInventory.Inventory.TrySpend(_targetKeyColor))
            {
                Open(heroInventory.transform.position);
            }
        }

        private void Open(Vector3 fromPosition)
        {
            _animator.Open(GetDirection(fromPosition));
            IsActivated = true;
        }

        private float GetDirection(Vector3 collectorPosition)
        {
            Vector3 directionToCollector = collectorPosition - _selfTransform.position;
            return Vector3.Dot(directionToCollector, _selfTransform.forward);
        }
    }
}