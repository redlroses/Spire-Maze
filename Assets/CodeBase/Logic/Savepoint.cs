﻿using CodeBase.Data;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Player;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(SavepointObserver))]
    [RequireComponent(typeof(BoxCollider))]
    public class Savepoint : ObserverTarget<SavepointObserver, Hero>, ISavedProgress, IIndexable
    {
        [SerializeField] private BoxCollider _collider;

        private ISaveLoadService _saveLoadService;

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public void Construct(int id, ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            Id = id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables.Find(cell => cell.Id == Id);

            if (cellState == null || cellState.IsActivated == false)
            {
                return;
            }

            IsActivated = cellState.IsActivated;
            SetColliderState(IsActivated);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables
                .Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.Indexables.Add(new IndexableState(Id, IsActivated));
            }
            else
            {
                cellState.IsActivated = IsActivated;
            }
        }

        protected override void OnTriggerObserverEntered(Hero damagable)
        {
            IsActivated = true;
            SetColliderState(IsActivated);
            _saveLoadService.SaveProgress();
        }

        private void SetColliderState(bool cellIsActive) =>
            _collider.enabled = cellIsActive == false;
    }
}