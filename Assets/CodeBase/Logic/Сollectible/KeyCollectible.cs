using CodeBase.Data;
using CodeBase.Data.CellStates;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    public class KeyCollectible : MonoBehaviour, ICollectible, IIndexable, ISavedProgress
    {
        [SerializeField] private Colors _color;
        [SerializeField] private MaterialChanger _materialChanger;

        private StorableStaticData _keyStaticStaticData;
        private bool _isTaken;
        private int _id;

        public int Id => _id;
        public Colors Color => _color;
        public StorableStaticData StorableStaticData => _keyStaticStaticData;

        public void Construct(IGameFactory gameFactory, StorableStaticData staticStaticData, Colors color, int id)
        {
            _materialChanger.Construct(gameFactory);
            _materialChanger.SetMaterial(color);
            _keyStaticStaticData = staticStaticData;
            _color = color;
            _id = id;
        }

        public void Disable()
        {
            _isTaken = true;
            gameObject.SetActive(false);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            KeyState cellState = progress.WorldData.LevelState.KeyStates.Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                return;
            }

            if (cellState.IsTaken)
            {
                Disable();
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            KeyState cellState = progress.WorldData.LevelState.KeyStates.Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.KeyStates.Add(new KeyState(Id, _isTaken));
            }
            else
            {
                cellState.IsTaken = _isTaken;
            }
        }
    }
}