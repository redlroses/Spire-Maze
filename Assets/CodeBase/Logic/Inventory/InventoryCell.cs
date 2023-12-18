using CodeBase.Logic.Items;
using UnityEngine;

namespace CodeBase.Logic.Inventory
{
    public sealed class InventoryCell : IReadOnlyInventoryCell
    {
        [field: SerializeField] public int Count { get; private set; }

        [field: SerializeField] public IItem Item { get; }

        public InventoryCell(IItem item)
        {
            Count = 1;
            Item = item;
        }

        public bool IsEmpty => Count <= 0;

        public InventoryCell(IItem item, int count)
        {
            Count = count;
            Item = item;
        }

        public void Increase() =>
            Count++;

        public void Decrease() =>
            Count--;
    }
}