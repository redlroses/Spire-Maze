using System.Linq;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public class LevelConstructor : MonoBehaviour
    {
        public const CellType Markers = CellType.MovingMarkerDown | CellType.MovingMarkerLeft |
                                        CellType.MovingMarkerRight | CellType.MovingMarkerUp;

        private readonly CellConstructor _cellConstructor = new CellConstructor();

        public void Construct(Level level)
        {
            _cellConstructor.Construct<Wall>(level.Where(cell => cell.CellType == CellType.Wall).ToArray());
            _cellConstructor.Construct<Plate>(level.Where(cell => (cell.CellType & CellType.Plate) == CellType.Plate).ToArray());
            _cellConstructor.Construct<Key>(level.Where(cell => cell.CellType == CellType.Key).ToArray());
            _cellConstructor.Construct<Door>(level.Where(cell => cell.CellType == CellType.Door).ToArray());
            _cellConstructor.Construct<MovingPlate>(level.Where(cell => cell.CellType == CellType.MovingPlate).ToArray());
            _cellConstructor.Construct<MovingPlateMarker>(level.Where(cell => (int) (cell.CellType & Markers) > 1).ToArray());
        }
    }
}