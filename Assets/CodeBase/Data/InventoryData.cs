using System;
using System.Collections.Generic;
using CodeBase.Logic.Inventory;

namespace CodeBase.Data
{
    [Serializable]
    public class InventoryData
    {
        public List<IReadOnlyInventoryCell> InventoryCells;

        public InventoryData()
        {
            InventoryCells = new List<IReadOnlyInventoryCell>();
        }

        public InventoryData(List<IReadOnlyInventoryCell> inventoryCells)
        {
            InventoryCells = inventoryCells;
        }
    }
}