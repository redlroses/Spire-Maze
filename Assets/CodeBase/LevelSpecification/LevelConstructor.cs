using System.Linq;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public class LevelConstructor : MonoBehaviour
    {
        private readonly CellConstructor _cellConstructor = new CellConstructor();

        public void Construct(Level level)
        {
            _cellConstructor.Construct<Wall>(level.Where(cell => cell.CellData is Data.Cell.Wall).ToArray());
            _cellConstructor.Construct<Plate>(level.Where(cell => cell.CellData is Data.Cell.Plate).ToArray());
            _cellConstructor.Construct<Key>(level.Where(cell => cell.CellData is Data.Cell.Key).ToArray());
            _cellConstructor.Construct<Door>(level.Where(cell => cell.CellData is Data.Cell.Door).ToArray());
            // _cellConstructor.Construct<MovingPlateMarker>(level.Where(cell => (int) (cell.CellType & CellType.MovingMarker) > 1).ToArray());
        }
    }
}