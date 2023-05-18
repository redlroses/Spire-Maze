using System;
using System.Collections.Generic;
using CodeBase.Logic.Items;
using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Inventory
{
    public interface IInventory : IEnumerable<IReadOnlyInventoryCell>
    {
        event Action Updated;
        int Count { get; }
        bool TryUse(StorableType storableType);
        void Cleanup();
        void Add(IItem collectible);
    }
}