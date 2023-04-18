using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Inventory
{
    public sealed class InventoryCell : IReadOnlyInventoryCell
    {
        public int Count { get; private set; }
        public StorableStaticData Item { get; }

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