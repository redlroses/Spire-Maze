using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using Rock = CodeBase.EditorCells.Rock;

namespace CodeBase.LevelSpecification.Constructor
{
    public class RockConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                Rock rockData = (Rock)cell.CellData;

                Logic.Trap.Rock rock = gameFactory.CreateCell<TCell>(cell.Container)
                    .GetComponentInChildren<Logic.Trap.Rock>();

                rock.Construct(cell.Id, rock.TrapActivator);
                rock.SetMoveDirection(rockData.IsDirectionToRight);
            }
        }
    }
}