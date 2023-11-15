using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.SaveLoad;
using Savepoint = CodeBase.Logic.Checkpoint.Savepoint;

namespace CodeBase.LevelSpecification.Constructor
{
    public class SavepointConstructor : ICellConstructor
    {
        private readonly ISaveLoadService _saveLoadService;

        public SavepointConstructor(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                Savepoint savepoint = gameFactory.CreateCell<TCell>(cell.Container).GetComponent<Savepoint>();
                savepoint.Construct(cell.Id, _saveLoadService);
            }
        }
    }
}