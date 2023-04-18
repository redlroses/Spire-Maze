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

        protected override void OnTriggerObserverEntered(ICollectible collectible)
        {
            _heroInventory.Inventory.Add(collectible.StorableStaticData);
            collectible.Disable();
        }
    }
}