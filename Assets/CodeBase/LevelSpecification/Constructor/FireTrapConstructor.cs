using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.Pause;
using UnityEngine;

namespace CodeBase.LevelSpecification.Constructor
{
    public class FireTrapConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (var cell in cells)
            {
                var fireTrap = gameFactory.CreateCell<TCell>(cell.Container)
                    .GetComponentInChildren<Logic.Trap.FireTrap>();
                fireTrap.Construct(cell.Id, fireTrap.TrapActivator);
            }
        }
    }
}