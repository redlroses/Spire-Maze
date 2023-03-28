using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;

namespace CodeBase.LevelSpecification.Constructor
{
    public class SavepointConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                var savepoint = gameFactory.CreateCell<TCell>(cell.Container).GetComponent<Logic.Savepoint>();
                savepoint.Construct(cell.Id);
            }
        }
    }
}