using System;
using CodeBase.Logic.Item;
using CodeBase.Logic.Сollectible;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Inventory
{
    [Serializable]
    public sealed class InventoryCell : IReadOnlyInventoryCell
    {
        [field: SerializeField] public int Count { get; private set; }
        [field: SerializeField] public IItem Item { get; private set; }

        public bool IsEmpty => Count <= 0;

        public InventoryCell(IItem item)
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