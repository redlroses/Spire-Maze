using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.StaticData;

namespace CodeBase.LevelSpecification.Constructor
{
    public class PlateConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, IStaticDataService staticData, Cell[] cells) where TCell : Cell
        {
            foreach (var cell in cells)
            {
                gameFactory.CreateCell<TCell>(cell.Container);
            }
        }
    }
}