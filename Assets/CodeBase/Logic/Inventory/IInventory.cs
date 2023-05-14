using System;
using System.Collections.Generic;
using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Inventory
{
    public interface IInventory : IEnumerable<IReadOnlyInventoryCell>
    {
        event Action Updated;
        int Count { get; }
        bool TryUse(StorableType storableType, out StorableStaticData item);
        void Cleanup();
        void Add(StorableStaticData collectible);
        List<InventoryCell> ReadAll();
    }
}