using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;

namespace CodeBase.LevelSpecification.Constructor
{
    public class WallConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
        {
            foreach (var cell in cells)
            {
                CellFactory.InstantiateCell<Wall>(cell.Container);
            }
        }
    }
}