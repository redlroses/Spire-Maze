using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class InventoryData
    {
        public List<ItemData> ItemsData;

        public InventoryData()
        {
            ItemsData = new List<ItemData>();
        }

        public InventoryData(List<ItemData> itemsData)
        {
            ItemsData = itemsData;
        }
    }
}