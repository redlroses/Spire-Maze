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
        public event Action SoundPlayed;

        protected override void OnTriggerObserverEntered(ICollectible collectible)
        {
            _heroInventory.Inventory.Add(collectible.Item);
            Collected.Invoke(collectible.Item.Sprite, transform.position);
            collectible.Disable();
            SoundPlayed?.Invoke();
        }
    }
}