using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Items;
using CodeBase.Logic.Сollectible;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Storable;
using ItemSpawnPoint = CodeBase.EditorCells.ItemSpawnPoint;

namespace CodeBase.LevelSpecification.Constructor
{
    public class ItemSpawnPointConstructor : ICellConstructor
    {
        private readonly IStaticDataService _staticDataService;

        public ItemSpawnPointConstructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                ItemSpawnPoint itemData = (ItemSpawnPoint)cell.CellData;
                Collectible collectible =
                    gameFactory.CreateCell<TCell>(cell.Container).GetComponentInChildren<Collectible>();
                StorableStaticData storableStaticData = _staticDataService.GetStorable(itemData.Type);
                IItem item = gameFactory.CreateItem(storableStaticData);
                collectible.Construct(cell.Id, item);
            }
        }
    }
}