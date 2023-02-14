using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Lift;

namespace CodeBase.LevelSpecification.Constructor
{
    public class MovingPlateMarkerConstructor : ICellConstructor
    {
        private readonly Dictionary<CellType, MovingDirection> _directions;

        public MovingPlateMarkerConstructor()
        {
            _directions = new Dictionary<CellType, MovingDirection>
            {
                [CellType.MovingMarkerLeft] = MovingDirection.Left,
                [CellType.MovingMarkerUp] = MovingDirection.Up,
                [CellType.MovingMarkerRight] = MovingDirection.Right,
                [CellType.MovingMarkerDown] = MovingDirection.Down,
            };
        }

        public void Construct<TCell>(Cell[] cells)
        {
            foreach (var cell in cells)
            {
                LiftDestinationMarker marker = CellFactory.InstantiateCell<MovingPlateMarker>(cell.Container).GetComponent<LiftDestinationMarker>();
                CellPosition cellPosition = new CellPosition(
                    cell.Container.position.y,
                    cell.Container.rotation.eulerAngles.y);
                MovingDirection movingDirection = _directions[cell.CellType & LevelConstructor.Markers];
                marker.Construct(cellPosition, movingDirection);
            }
        }

        private void FindPair(Cell current)
        {
        }
    }
}