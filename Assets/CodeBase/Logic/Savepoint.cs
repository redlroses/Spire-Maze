using CodeBase.Data;
using CodeBase.Data.CellStates;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Player;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(SavepointObserver))]
    [RequireComponent(typeof(BoxCollider))]
    public class Savepoint : ObserverTarget<SavepointObserver, Hero>, ISavedProgress
    {
        [SerializeField] private BoxCollider _collider;

        private int _id;
        private bool _isActive;

        public int Id => _id;

        public void Construct(int id)
        {
            _id = id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            SavepointState cellState = progress.WorldData.LevelState.SavepointStates
                .Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                return;
            }

            if (cellState.IsActive)
            {
                _isActive = cellState.IsActive;
                SetColliderState(cellState.IsActive);
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            SavepointState cellState = progress.WorldData.LevelState.SavepointStates
                .Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.SavepointStates.Add((new SavepointState(Id, _isActive)));
            }
            else
            {
                cellState.IsActive = _isActive;
            }
        }

        protected override void OnTriggerObserverEntered(Hero player)
        {
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
        }

        private void SetColliderState(bool cellIsActive) => _collider.enabled = cellIsActive == false;
    }
}