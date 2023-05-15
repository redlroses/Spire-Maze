using CodeBase.Data;
using CodeBase.Logic.Сollectible;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Inventory
{
    public class CompassCollectible : MonoBehaviour, ICollectible, IIndexable, ISavedProgress, IUsable
    {
        private StorableStaticData _compassStaticData;
        private bool _isTaken;
        private int _id;

        public int Id => _id;
        public StorableStaticData StorableStaticData => _compassStaticData;
        
        public void Construct(StorableStaticData staticStaticData, int id)
        {
            _compassStaticData = staticStaticData;
            _id = id;
        }
        
        public void Disable()
        {
            _isTaken = true;
            gameObject.SetActive(false);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            throw new System.NotImplementedException();
        }

        public void Use()
        {
            Debug.Log($"Used {_compassStaticData.ItemType}");
        }
    }
}