using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data.Cell;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Lift;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Tools.Constants;
using CodeBase.Tools.Extension;
using MovingPlate = CodeBase.LevelSpecification.Cells.MovingPlate;

namespace CodeBase.LevelSpecification.Constructor
{
    public class MovingPlateMarkerConstructor : ICellConstructor
    {
        private Cell[] _markers;
        private Cell[] _movingPlates;

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
        {
            _markers = cells;
            _movingPlates = cells.Where(cell => ((MovingMarker) cell.CellData).IsLiftHolder).ToArray();

            foreach (Cell cell in _movingPlates)
            {
                CellFactory.InstantiateCell<MovingPlate>(cell.Container);
            }

            foreach (Cell cell in _markers)
            {
                CellFactory.InstantiateCell<MovingPlateMarker>(cell.Container);
            }

            foreach (Cell movingPlates in _movingPlates)
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

            initialMarker.Construct(movingPlate.Position);
            destinationMarker.Construct(pairMarker.Position);


            PlateMoveDirection plateMoveDirection = ((MovingMarker) movingPlate.CellData).Direction;
            IPlateMover mover = plateMoveDirection == PlateMoveDirection.Left || plateMoveDirection == PlateMoveDirection.Right
                ? (IPlateMover) liftPlate.gameObject.AddComponent<PlateHorizontalMover>()
                : liftPlate.gameObject.AddComponent<PlateVerticalMover>();

            liftPlate.Construct(initialMarker, destinationMarker, mover);
        }

        private Cell FindPair(Cell movingPlateCell)
        {
            PlateMoveDirection plateMoveDirection = ((MovingMarker) movingPlateCell.CellData).Direction;
            return plateMoveDirection == PlateMoveDirection.Left || plateMoveDirection == PlateMoveDirection.Right
                ? FindHorizontalPair(movingPlateCell)
                : FindVerticalPair(movingPlateCell);
        }

        private Cell FindVerticalPair(Cell movingPlateCell)
        {
            float movingPlateHeight = movingPlateCell.Position.Height;

            if (((MovingMarker) movingPlateCell.CellData).Direction == PlateMoveDirection.Up)
            {
                List<Cell> filtered = GetFilteredByAngle(movingPlateCell, PlateMoveDirection.Down);
                return GetClosestDownPair(filtered, movingPlateHeight);
            }
            else
            {
                List<Cell> filtered = GetFilteredByAngle(movingPlateCell, PlateMoveDirection.Up);
                return GetClosestUpPair(filtered, movingPlateHeight);
            }
        }

        private Cell FindHorizontalPair(Cell movingPlateCell)
        {
            float movingPlateAngle = movingPlateCell.Position.Angle;

            if (((MovingMarker) movingPlateCell.CellData).Direction == PlateMoveDirection.Left)
            {
                List<Cell> filtered = GetFilteredByHeight(movingPlateCell, PlateMoveDirection.Right);
                return GetClosestRightPair(filtered, movingPlateAngle);
            }
            else
            {
                List<Cell> filtered = GetFilteredByHeight(movingPlateCell, PlateMoveDirection.Left);
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

        private List<Cell> GetFilteredByHeight(Cell movingPlateCell, PlateMoveDirection type)
        {
            List<Cell> filtered =
                _markers.Where(cell =>
                    cell.Position.Height.EqualsApproximately(movingPlateCell.Position.Height) &&
                    ((MovingMarker) cell.CellData).Direction == type).ToList();
            return filtered;
        }

        private List<Cell> GetFilteredByAngle(Cell movingPlateCell, PlateMoveDirection type)
        {
            List<Cell> filtered =
                _markers.Where(cell =>
                    cell.Position.Angle.EqualsApproximately(movingPlateCell.Position.Angle) &&
                    ((MovingMarker) cell.CellData).Direction == type).ToList();
            return filtered;
        }
    }
}