using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
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