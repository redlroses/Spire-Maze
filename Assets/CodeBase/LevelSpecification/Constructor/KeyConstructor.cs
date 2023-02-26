using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Сollectible;

namespace CodeBase.LevelSpecification.Constructor
{
    public class KeyConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
        {
            foreach (var cell in cells)
            {
                var keyData = (Data.Cell.Key) cell.CellData;
                KeyCollectible key = CellFactory.InstantiateCell<Key>(cell.Container).GetComponent<KeyCollectible>();
                key.Construct(gameFactory, keyData.Color);
            }
        }
    }
}