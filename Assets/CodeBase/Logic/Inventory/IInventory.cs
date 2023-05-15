using System;
using System.Collections.Generic;
using CodeBase.Logic.Item;
using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Inventory
{
    public interface IInventory : IEnumerable<IReadOnlyInventoryCell>
    {
        event Action Updated;
        int Count { get; }
        bool TryUse(StorableType storableType);
        bool TrySpend(StorableType storableType);
        void Cleanup();
        void Add(IItem collectible);
        List<InventoryCell> ReadAll();
    }
}