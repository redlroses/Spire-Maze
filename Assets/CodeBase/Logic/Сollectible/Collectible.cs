using System;
using CodeBase.Data;
using CodeBase.Logic.Items;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    public class Collectible : MonoBehaviour, ICollectible, IIndexable, ISavedProgress
    {
        [SerializeField] private Collider _trigger;

        public IItem Item { get; private set; }
        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public event Action Collected = () => { };

        protected virtual void OnConstruct(IItem item) { }
        protected virtual void OnLoadState(bool isActivated) { }
        protected virtual void OnCollected() { }

        public void Construct(int id, IItem item)
        {
            Id = id;
            Item = item;
            OnConstruct(item);
        }

        public void Collect()
        {
            IsActivated = true;
            _trigger.enabled = false;
            OnCollected();
            Collected.Invoke();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables.Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                return;
            }

            _trigger.enabled = !cellState.IsActivated;
            IsActivated = cellState.IsActivated;
            OnLoadState(cellState.IsActivated);
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
    }
}