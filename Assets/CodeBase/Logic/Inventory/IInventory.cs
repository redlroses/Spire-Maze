using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Inventory
{
    public interface IInventory : IEnumerable
    {
        event Action Updated;
        bool TryUse(StorableType storableType, out StorableStaticData item);
        void Cleanup();
        void Add(StorableStaticData collectible);
        List<InventoryCell> ReadAll();
    }
}