using System;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Inventory
{
    [Serializable]
    public sealed class InventoryCell : IReadOnlyInventoryCell
    {
        [field: SerializeField] public int Count { get; private set; }
        [field: SerializeField] public StorableStaticData Item { get; private set; }

        public bool IsEmpty => Count <= 0;

        public InventoryCell(StorableStaticData item)
        {
            Count = 1;
            Item = item;
        }

        public void IncreaseCount()
        {
            Count++;
        }

        public void DecreaseCount()
        {
            Count--;
        }
    }
}