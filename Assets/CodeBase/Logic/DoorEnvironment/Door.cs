﻿using CodeBase.Data;
using CodeBase.Data.CellStates;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Сollectible;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.DoorEnvironment
{
    [RequireComponent(typeof(KeyCollectorObserver))]
    public class Door : ObserverTarget<KeyCollectorObserver, KeyCollector>, ISavedProgress, IIndexable
    {
        [SerializeField] private Colors _doorColor;
        [SerializeField] private DoorAnimator _animator;
        [SerializeField] private MaterialChanger _materialChanger;
        [SerializeField] private bool _isOpen;

        private Transform _selfTransform;
        private int _id;

        public int Id => _id;

        public void Construct(IGameFactory gameFactory, Colors doorDataColor, int id)
        {
            _materialChanger.Construct(gameFactory);
            _materialChanger.SetMaterial(doorDataColor);
            _doorColor = doorDataColor;
            _selfTransform = transform;
            _id = id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            DoorState cellState = progress.WorldData.LevelState.DoorStates.Find(cell => cell.Id == Id);

            if (cellState == null || cellState.IsOpen == false)
            {
                return;
            }

            _animator.Open(1f);
            _isOpen = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            DoorState cellState = progress.WorldData.LevelState.DoorStates.Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.DoorStates.Add(new DoorState(Id, _isOpen));
            }
            else
            {
                cellState.IsOpen = _isOpen;
            }
        }

        protected override void OnTriggerObserverEntered(KeyCollector collectible)
        {
            if (collectible.TryUseKey(_doorColor))
            {
                Open(collectible);
            }
        }

        private void Open(KeyCollector keyCollector)
        {
            _animator.Open(GetDirection(keyCollector.transform.position));
            _isOpen = true;
        }

        private float GetDirection(Vector3 collectorPosition)
        {
            Vector3 directionToCollector = collectorPosition - _selfTransform.position;
            return Vector3.Dot(directionToCollector, _selfTransform.forward);
        }
    }
}