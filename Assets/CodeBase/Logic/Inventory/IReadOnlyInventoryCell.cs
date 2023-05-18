using CodeBase.Logic.Items;

namespace CodeBase.Logic.Inventory
{
    public interface IReadOnlyInventoryCell
    {
        int Count { get; }
        IItem Item { get; }
    }
}