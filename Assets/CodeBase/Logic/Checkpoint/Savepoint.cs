using System;
using CodeBase.Data;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Player;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic.Checkpoint
{
    [RequireComponent(typeof(SavepointObserver))]
    [RequireComponent(typeof(BoxCollider))]
    public class Savepoint : ObserverTarget<SavepointObserver, HeroRoot>, ISavedProgress, IIndexable
    {
        [SerializeField] private BoxCollider _collider;

        private ISaveLoadService _saveLoadService;

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public event Action<bool> Activated = _ => { };

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
            Activated.Invoke(false);
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

        protected override void OnTriggerObserverEntered(HeroRoot _)
        {
            IsActivated = true;
            SetColliderState(true);
            Activated.Invoke(true);
            _saveLoadService.SaveProgress();
        }

        private void SetColliderState(bool cellIsActive) =>
            _collider.enabled = cellIsActive == false;
    }
}