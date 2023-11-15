using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.LevelSpecification.Cells;
using FinishPortal = CodeBase.Logic.FinishPortal;

namespace CodeBase.LevelSpecification.Constructor
{
    public class FinishPortalConstructor : ICellConstructor
    {
        private readonly GameStateMachine _stateMachine;

        public FinishPortalConstructor(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                gameFactory.CreateCell<TCell>(cell.Container).GetComponent<FinishPortal>().Construct(_stateMachine);
            }
        }
    }
}