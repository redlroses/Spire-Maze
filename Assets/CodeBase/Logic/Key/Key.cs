using AYellowpaper.SerializedCollections;
using CodeBase.Logic.Items;
using CodeBase.Logic.Сollectible;
using CodeBase.StaticData.Storable;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic.Key
{
    public class Key : Collectible
    {
        [SerializeField] private KeyAnimator _animator;
        [SerializeField] private SerializedDictionary<StorableType, GameObject> _objects;

        public override void Construct(int id, IItem item)
        {
            base.Construct(id, item);
            EnableMark(item.ItemType);
        }

        protected override void OnCollected() => 
            gameObject.SetActive(false);

        private void EnableMark(StorableType type)
        {
            _objects[type].SetActive(true);
            _animator.SetColor(type);
        }
    }
}