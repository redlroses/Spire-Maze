using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Inventory
{
    public interface IReadOnlyInventoryCell
    {
        int Count { get; }
        IStorable Item { get; }
    }
}