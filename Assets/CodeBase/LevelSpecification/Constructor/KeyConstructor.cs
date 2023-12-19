using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Сollectible;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;
using Key = CodeBase.EditorCells.Key;

namespace CodeBase.LevelSpecification.Constructor
{
    public class KeyConstructor : ICellConstructor
    {
        private readonly IStaticDataService _staticDataService;

        public KeyConstructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                Key keyData = (Key)cell.CellData;
                Collectible collectible = gameFactory.CreateCell<TCell>(cell.Container).GetComponent<Collectible>();
                collectible.Construct(cell.Id,
                    gameFactory.CreateItem(_staticDataService.GetStorable(keyData.Color.ToStorableType())));
            }
        }
    }
}