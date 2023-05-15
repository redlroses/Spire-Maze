using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;

namespace CodeBase.LevelSpecification.Constructor
{
    public class PlateConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            foreach (var cell in cells)
            {
                gameFactory.CreateCell<TCell>(cell.Container);
            }
        }
    }
}