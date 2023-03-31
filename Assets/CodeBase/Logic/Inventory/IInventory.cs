using System;
using System.Collections.Generic;
using CodeBase.Logic.Inventory.Storables;

namespace CodeBase.Logic.Inventory
{
    public interface IInventory
    {
        event Action Updated;
        bool TryUse<TItem>(out TItem item) where TItem : IStorable;
        void ClearUp();
        void Add<TItem>(IStorable collectible) where TItem : IStorable;
        List<InventoryCell> ReadAll();
    }
}