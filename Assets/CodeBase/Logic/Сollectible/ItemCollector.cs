using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    [RequireComponent(typeof(CollectibleObserver))]
    public abstract class ItemCollector<TItem> : ObserverTarget<CollectibleObserver, ICollectible>
    {
        protected override void OnTriggerObserverEntered(ICollectible collectible)
        {
            if (collectible is TItem item)
            {
                Collect(item);
                collectible.Disable();
            }
        }

        protected abstract void Collect(TItem item);
    }
}