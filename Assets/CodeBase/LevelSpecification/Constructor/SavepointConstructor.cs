using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.SaveLoad;

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
            foreach (var cell in cells)
            {
                var savepoint = gameFactory.CreateCell<TCell>(cell.Container).GetComponent<Logic.Savepoint>();
                savepoint.Construct(cell.Id, _saveLoadService);
            }
        }
    }
}