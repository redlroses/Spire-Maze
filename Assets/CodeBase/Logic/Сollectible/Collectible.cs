using CodeBase.Data;
using CodeBase.Logic.Item;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    public class Collectible : MonoBehaviour, ICollectible, IIndexable, ISavedProgress
    {
        public IItem Item { get; private set; }
        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public void Construct(int id, IItem item)
        {
            Id = id;
            Item = item;
        }

        public void Disable()
        {
            IsActivated = true;
            gameObject.SetActive(false);
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
                Disable();
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
    }
}