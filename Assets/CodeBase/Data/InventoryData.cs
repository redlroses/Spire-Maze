using System;
using System.Collections.Generic;
using CodeBase.Logic.Inventory;

namespace CodeBase.Data
{
    [Serializable]
    public class InventoryData
    {
        public List<InventoryCell> InventoryCells;

        public InventoryData()
        {
            InventoryCells = new List<InventoryCell>();
        }

        public InventoryData(List<InventoryCell> inventoryCells)
        {
            InventoryCells = inventoryCells;
        }
    }
}