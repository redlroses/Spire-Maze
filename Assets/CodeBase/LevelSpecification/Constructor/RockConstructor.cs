using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using Rock = CodeBase.Logic.Trap.Rock;

namespace CodeBase.LevelSpecification.Constructor
{
    public class RockConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (var cell in cells)
            {
                EditorCells.Rock rockData = (EditorCells.Rock) cell.CellData;
                Rock rock = gameFactory.CreateCell<TCell>(cell.Container).GetComponentInChildren<Rock>();
                rock.Construct(cell.Id, rock.TrapActivator);
                rock.SetMoveDirection(rockData.IsDirectionToRight);
            }
        }
    }
}