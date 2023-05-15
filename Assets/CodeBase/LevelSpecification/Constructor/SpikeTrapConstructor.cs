using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.Pause;

namespace CodeBase.LevelSpecification.Constructor
{
    public class SpikeTrapConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            foreach (var cell in cells)
            {
                var spikeTrap = gameFactory.CreateCell<TCell>(cell.Container)
                    .GetComponentInChildren<Logic.Trap.Spike>();
                spikeTrap.Construct(cell.Id, spikeTrap.TrapActivator);
            }
        }
    }
}