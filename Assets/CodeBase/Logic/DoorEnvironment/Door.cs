using System;
using AYellowpaper.SerializedCollections;
using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Logic.Inventory;
using CodeBase.Logic.Observer;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Storable;
using CodeBase.Tools.Extension;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.DoorEnvironment
{
    [RequireComponent(typeof(InventoryObserver))]
    public class Door : ObserverTarget<InventoryObserver, HeroInventory>, ISavedProgress, IIndexable
    {
        [SerializeField] private DoorAnimator _animator;
        [SerializeField] private SerializedDictionary<StorableType, GameObject> _keySigns;

        private StorableType _targetKeyColor;

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public void Construct(Colors doorColor, int id)
        {
            _targetKeyColor = doorColor.ToStorableType();
            Id = id;
            EnableSign();
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
                _animator.Open();
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

        protected override void OnTriggerObserverEntered(HeroInventory damagable)
        {
            if (damagable.Inventory.TryUse(_targetKeyColor))
            {
                Open();
            }
        }

        private void EnableSign() =>
            _keySigns[_targetKeyColor].Enable();

        private void Open()
        {
            _animator.Open();
            IsActivated = true;
        }
    }
}