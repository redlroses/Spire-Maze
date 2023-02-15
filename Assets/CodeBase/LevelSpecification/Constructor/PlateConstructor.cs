using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;

namespace CodeBase.LevelSpecification.Constructor
{
    public class PlateConstructor : ICellConstructor
    {
        public void Construct<TCell>(Cell[] cells)
        {
            foreach (var cell in cells)
            {
                CellFactory.InstantiateCell<Plate>(cell.Container);
            }
        }
    }
}