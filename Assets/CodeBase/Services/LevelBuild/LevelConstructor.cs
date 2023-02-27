using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;

namespace CodeBase.Services.LevelBuild
{
    public class LevelConstructor
    {
        private readonly CellConstructor _cellConstructor = new CellConstructor();

        public void Construct(IGameFactory gameFactory, Level level)
        {
            _cellConstructor.Construct<Wall>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.Wall).ToArray());
            _cellConstructor.Construct<Plate>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.Plate).ToArray());
            _cellConstructor.Construct<Key>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.Key).ToArray());
            _cellConstructor.Construct<Door>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.Door).ToArray());
            _cellConstructor.Construct<MovingPlateMarker>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.MovingMarker).ToArray());
        }
    }
}