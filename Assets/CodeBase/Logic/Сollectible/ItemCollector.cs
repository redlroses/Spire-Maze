using System;
using CodeBase.Logic.Inventory;
using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    [RequireComponent(typeof(CollectibleObserver))]
    [RequireComponent(typeof(HeroInventory))]
    public class ItemCollector : ObserverTarget<CollectibleObserver, ICollectible>
    {
        [SerializeField] private HeroInventory _heroInventory;

        public event Action<Sprite, Vector3> Collected = (_, _) => { };

        protected override void OnTriggerObserverEntered(ICollectible damagable)
        {
            _heroInventory.Inventory.Add(damagable.Item);
            Collected.Invoke(damagable.Item.Sprite, transform.position);
            damagable.Collect();
        }
    }
}