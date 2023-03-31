using CodeBase.Logic.Inventory.Storables;

namespace CodeBase.Logic.Inventory
{
    public interface IReadOnlyInventoryCell
    {
        int Count { get; }
        IStorable Item { get; }
    }
}