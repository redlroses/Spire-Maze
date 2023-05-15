using System;
using CodeBase.Data;
using CodeBase.Logic.Item;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    public class CompassCollectible : MonoBehaviour, IIndexable, ISavedProgress, IUsable
    {
        private StorableStaticData _compassStaticData;

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public StorableStaticData StorableStaticData => _compassStaticData;

        public void Construct(StorableStaticData staticStaticData, int id)
        {
            _compassStaticData = staticStaticData;
            Id = id;
        }

        public void Disable()
        {
            IsActivated = true;
            gameObject.SetActive(false);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables
                .Find(cell => cell.Id == Id);

            if (cellState == null || cellState.IsActivated == false)
            {
                return;
            }

            IsActivated = cellState.IsActivated;

            if (cellState.IsActivated)
            {
                throw new NotImplementedException();
            }
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

        public void Use()
        {
            Debug.Log($"Used {_compassStaticData.ItemType}");
        }
    }
}