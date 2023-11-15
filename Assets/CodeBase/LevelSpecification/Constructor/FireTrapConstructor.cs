using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using FireTrap = CodeBase.Logic.Trap.FireTrap;

namespace CodeBase.LevelSpecification.Constructor
{
    public class FireTrapConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                FireTrap fireTrap = gameFactory.CreateCell<TCell>(cell.Container)
                    .GetComponentInChildren<FireTrap>();
                fireTrap.Construct(fireTrap.TrapActivator);
            }
        }
    }
}