using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;

namespace CodeBase.LevelSpecification.Constructor
{
    public class DoorConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
        {
            foreach (var cell in cells)
            {
                var doorData = (EditorCells.Door) cell.CellData;
                var door = gameFactory.CreateCell<Door>(cell.Container).GetComponent<Logic.DoorEnvironment.Door>();
                door.Construct(gameFactory, doorData.Color, cell.Id);
            }
        }
    }
}