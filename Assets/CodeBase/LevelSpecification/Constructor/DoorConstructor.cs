using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using Door = CodeBase.EditorCells.Door;

namespace CodeBase.LevelSpecification.Constructor
{
    public class DoorConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                Door doorData = (Door)cell.CellData;
                Logic.DoorEnvironment.Door door = gameFactory.CreateCell<TCell>(cell.Container)
                    .GetComponent<Logic.DoorEnvironment.Door>();
                door.Construct(doorData.Color, cell.Id);
            }
        }
    }
}