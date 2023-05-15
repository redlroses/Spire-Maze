using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    public class KeyCollectible : MonoBehaviour, ICollectible, IIndexable, ISavedProgress
    {
        [SerializeField] private MaterialChanger _materialChanger;

        private StorableStaticData _keyStaticStaticData;

        public int Id { get; private set; }

        public bool IsActivated { get; private set; }

        public StorableStaticData StorableStaticData => _keyStaticStaticData;

        public void Construct(IGameFactory gameFactory, StorableStaticData staticStaticData, Colors color, int id)
        {
            _materialChanger.Construct(gameFactory);
            _materialChanger.SetMaterial(color);
            _keyStaticStaticData = staticStaticData;
            Id = id;
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