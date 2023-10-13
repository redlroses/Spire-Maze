using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class InventoryData
    {
        public List<ItemData> ItemDatas = new List<ItemData>();

        public InventoryData()
        {
            //InventoryCells = new List<IReadOnlyInventoryCell>();
        }

        public InventoryData(List<ItemData> itemDatas)
        {
            ItemDatas = itemDatas;
            Debug.Log("Inventory from list");
        }
    }
}