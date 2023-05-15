using CodeBase.Logic.Item;

namespace CodeBase.Logic.Inventory
{
    public interface IReadOnlyInventoryCell
    {
        int Count { get; }
        IItem Item { get; }
    }
}