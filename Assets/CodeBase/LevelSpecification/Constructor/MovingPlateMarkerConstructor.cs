using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Lift;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Tools.Constants;
using CodeBase.Tools.Extension;

namespace CodeBase.LevelSpecification.Constructor
{
    public class MovingPlateMarkerConstructor : ICellConstructor
    {
        private readonly Dictionary<CellType, MovingDirection> _directions;

        private Cell[] _cells;
        private Cell[] _markers;
        private Cell[] _movingPlates;

        public MovingPlateMarkerConstructor()
        {
            _directions = new Dictionary<CellType, MovingDirection>
            {
                [CellType.Left] = MovingDirection.Left,
                [CellType.Up] = MovingDirection.Up,
                [CellType.Right] = MovingDirection.Right,
                [CellType.Down] = MovingDirection.Down,
            };
        }

        public void Construct<TCell>(Cell[] cells)
        {
            _cells = cells;
            _markers = cells.Where(cell =>
                cell.IsTypeOf(CellType.MovingMarker, CellType.MovingPlate)).ToArray();
            _movingPlates = cells.Where(cell => cell.IsTypeOf(CellType.MovingPlate)).ToArray();

            foreach (var cell in _movingPlates)
            {
                CellFactory.InstantiateCell<MovingPlate>(cell.Container);
            }

            foreach (var cell in _cells.Where(cell => cell.IsTypeOf(CellType.MovingMarker)))
            {
                CellFactory.InstantiateCell<MovingPlateMarker>(cell.Container);
            }

            foreach (var movingPlates in _movingPlates)
            {
                ApplyMovingPlate(movingPlates);
            }
        }

        private void ApplyMovingPlate(Cell movingPlate)
        {
            Cell pairMarker = FindPair(movingPlate);
            LiftDestinationMarker initialMarker = movingPlate.Container.GetComponentInChildren<LiftDestinationMarker>();
            LiftDestinationMarker destinationMarker = pairMarker.Container.GetComponentInChildren<LiftDestinationMarker>();
            LiftPlate liftPlate = movingPlate.Container.GetComponentInChildren<LiftPlate>();

            // initialMarker.Construct(movingPlate.Position, _directions[movingPlate.CellType & CellType.MovingMarker]);
            // destinationMarker.Construct(pairMarker.Position, _directions[pairMarker.CellType & CellType.MovingMarker]);

            IPlateMover mover = movingPlate.IsTypeOf(CellType.Left | CellType.Right)
                ? (IPlateMover) liftPlate.gameObject.AddComponent<PlateHorizontalMover>()
                : liftPlate.gameObject.AddComponent<PlateVerticalMover>();

            liftPlate.Construct(initialMarker, destinationMarker, mover);
        }

        private Cell FindPair(Cell movingPlateCell) =>
            movingPlateCell.IsTypeOf(CellType.Right | CellType.Left)
                ? FindHorizontalPair(movingPlateCell)
                : FindVerticalPair(movingPlateCell);

        private Cell FindVerticalPair(Cell movingPlateCell)
        {
            float movingPlateHeight = movingPlateCell.Position.Height;

            if (movingPlateCell.IsTypeOf(CellType.Up))
            {
                List<Cell> filtered = GetFilteredByAngle(movingPlateCell, CellType.Down);
                return GetClosestDownPair(filtered, movingPlateHeight);
            }
            else
            {
                List<Cell> filtered = GetFilteredByAngle(movingPlateCell, CellType.Up);
                return GetClosestUpPair(filtered, movingPlateHeight);
            }
        }

        private Cell FindHorizontalPair(Cell movingPlateCell)
        {
            float movingPlateAngle = movingPlateCell.Position.Angle;

            if (movingPlateCell.IsTypeOf(CellType.Left))
            {
                List<Cell> filtered = GetFilteredByHeight(movingPlateCell, CellType.Right);
                return GetClosestRightPair(filtered, movingPlateAngle);
            }
            else
            {
                List<Cell> filtered = GetFilteredByHeight(movingPlateCell, CellType.Left);
                return GetClosestLeftPair(filtered, movingPlateAngle);
            }
        }

        private Cell GetClosestDownPair(List<Cell> filtered, float movingPlateHeight)
        {
            return filtered.Where(cell => cell.Position.Height > movingPlateHeight)
                .OrderBy(cell => cell.Position.Height).Min();
        }

        private Cell GetClosestUpPair(List<Cell> filtered, float movingPlateHeight)
        {
            return filtered.Where(cell => cell.Position.Height < movingPlateHeight)
                .OrderBy(cell => cell.Position.Height).Max();
        }

        private Cell GetClosestLeftPair(IReadOnlyCollection<Cell> filtered, float movingPlateAngle)
        {
            List<Cell> possibleMarkers = null;
            bool isIncreaseAngle = false;

            for (int i = 0; i <= 1; i++)
            {
                possibleMarkers = filtered.Where(cell =>
                    cell.Position.Angle - Trigonometry.TwoPiGrade * i - movingPlateAngle < 0).ToList();

                if (possibleMarkers.Any())
                {
                    break;
                }

                isIncreaseAngle = true;
            }

            Cell target = (possibleMarkers ?? throw new ArgumentNullException(nameof(possibleMarkers)))
                .OrderBy(cell => cell.Position.Angle).Last();

            if (isIncreaseAngle)
            {
                target.RemoveTwoPiFromAngle();
            }

            return target;
        }

        private Cell GetClosestRightPair(IReadOnlyCollection<Cell> filtered, float movingPlateAngle)
        {
            List<Cell> possibleMarkers = null;
            bool isIncreaseAngle = false;

            for (int i = 0; i <= 1; i++)
            {
                possibleMarkers = filtered.Where(cell =>
                    cell.Position.Angle + Trigonometry.TwoPiGrade * i - movingPlateAngle > 0).ToList();

                if (possibleMarkers.Any())
                {
                    break;
                }

                isIncreaseAngle = true;
            }

            Cell target = (possibleMarkers ?? throw new ArgumentNullException(nameof(possibleMarkers)))
                .OrderBy(cell => cell.Position.Angle).First();

            if (isIncreaseAngle)
            {
                target.AddTwoPiToAngle();
            }

            return target;
        }

        private List<Cell> GetFilteredByHeight(Cell movingPlateCell, CellType type = CellType.All)
        {
            List<Cell> filtered =
                _markers.Where(cell =>
                    cell.Position.Height.EqualsApproximately(movingPlateCell.Position.Height) &&
                    cell.IsTypeOf(type)).ToList();
            return filtered;
        }

        private List<Cell> GetFilteredByAngle(Cell movingPlateCell, CellType type = CellType.All)
        {
            List<Cell> filtered =
                _markers.Where(cell =>
                    cell.Position.Angle.EqualsApproximately(movingPlateCell.Position.Angle) &&
                    cell.IsTypeOf(type)).ToList();
            return filtered;
        }
    }
}