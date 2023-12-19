using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Trap;

namespace CodeBase.LevelSpecification.Constructor
{
    public class SpikeTrapConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                Spike spikeTrap =
                    gameFactory.CreateCell<TCell>(cell.Container).GetComponentInChildren<Spike>();

                spikeTrap.Construct(spikeTrap.TrapActivator);
            }
        }
    }
}