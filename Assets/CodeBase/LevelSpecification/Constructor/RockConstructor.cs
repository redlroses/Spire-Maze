using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.Pause;
using CodeBase.Services.StaticData;
using Rock = CodeBase.Logic.Trap.Rock;

namespace CodeBase.LevelSpecification.Constructor
{
    public class RockConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, IStaticDataService staticData, Cell[] cells) where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                var rockData = (EditorCells.Rock) cell.CellData;
                Rock rock = gameFactory.CreateCell<TCell>(cell.Container).GetComponentInChildren<Rock>();
                rock.Construct(cell.Id, rock.TrapActivator);
                rock.SetMoveDirection(rockData.IsDirectionToRight);
            }
        }
    }
}