using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;

namespace CodeBase.LevelSpecification.Constructor
{
    public class DoorConstructor : ICellConstructor
    {
        public void Construct<TCell>(Cell[] cells)
        {
            foreach (var cell in cells)
            {
                CellFactory.InstantiateCell<Door>(cell.Container);
            }
        }
    }
}