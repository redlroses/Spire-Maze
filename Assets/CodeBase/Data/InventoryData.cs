using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class InventoryData
    {
        public List<ItemData> ItemDatas = new List<ItemData>();

        public InventoryData() { }

        public InventoryData(List<ItemData> itemDatas)
        {
            ItemDatas = itemDatas;
        }
    }
}