using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Сollectible;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Storable;
using Key = CodeBase.EditorCells.Key;

namespace CodeBase.LevelSpecification.Constructor
{
    public class KeyConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, IStaticDataService staticData, Cell[] cells) where TCell : Cell
        {
            foreach (var cell in cells)
            {
                var keyData = (Key) cell.CellData;
                KeyCollectible key = gameFactory.CreateCell<TCell>(cell.Container).GetComponent<KeyCollectible>();
                key.Construct(gameFactory, staticData.ForStorable(StorableType.Key), keyData.Color, cell.Id);
            }
        }
    }
}