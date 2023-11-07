using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Lift;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Tools.Constants;
using CodeBase.Tools.Extension;
using MovingPlate = CodeBase.LevelSpecification.Cells.MovingPlate;
using Object = UnityEngine.Object;

namespace CodeBase.LevelSpecification.Constructor
{
    public class MovingPlateMarkerConstructor : ICellConstructor
    {
        private Cell[] _markers;
        private Cell[] _movingPlates;

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            _markers = cells;
            _movingPlates = cells.Where(cell => ((MovingMarker) cell.CellData).IsLiftHolder).ToArray();

            foreach (Cell cell in _movingPlates)
                gameFactory.CreateCell<MovingPlate>(cell.Container);

            foreach (Cell cell in _markers)
                gameFactory.CreateCell<MovingPlateMarker>(cell.Container);

            foreach (Cell fromCell in _movingPlates)
            {
                Cell toCell = FindPair(fromCell);
                // CreateRails(fromCell, toCell);
                ApplyDestinationMarkers(fromCell, toCell);
            }
        }

        // private void CreateRails(Cell fromCell, Cell toCell)
        // {
        //     MovingMarker initialMarker = (MovingMarker) fromCell.CellData;
        //
        //     if (initialMarker.Direction == PlateMoveDirection.Left)
        //     {
        //         MovingMarker destinationMarker = (MovingMarker) toCell.CellData;
        //         
        //         for (int i = fromCell.Position.Angle; i < _markers.Length; i++)
        //         {
        //             fromCell.Container.r
        //         }
        //     }
        // }

        private void ApplyDestinationMarkers(Cell fromCell, Cell toCell)
        {
            LiftDestinationMarker initialMarker = fromCell.Container.GetComponentInChildren<LiftDestinationMarker>();
            LiftDestinationMarker destinationMarker = toCell.Container.GetComponentInChildren<LiftDestinationMarker>();
            LiftPlate liftPlate = fromCell.Container.GetComponentInChildren<LiftPlate>();

            initialMarker.Construct(fromCell.Position);
            destinationMarker.Construct(toCell.Position);

            PlateMoveDirection plateMoveDirection = ((MovingMarker) fromCell.CellData).Direction;
            IPlateMover mover = ChooseMoverComponent(liftPlate, plateMoveDirection);

            liftPlate.Construct(initialMarker, destinationMarker, mover);
        }

        private IPlateMover ChooseMoverComponent(LiftPlate liftPlate, PlateMoveDirection direction)
        {
            if (direction == PlateMoveDirection.Left || direction == PlateMoveDirection.Right)
            {
                Object.Destroy(liftPlate.gameObject.GetComponent<PlateVerticalMover>());
                return liftPlate.gameObject.GetComponent<PlateHorizontalMover>();
            }

            Object.Destroy(liftPlate.gameObject.GetComponent<PlateHorizontalMover>());
            return liftPlate.gameObject.GetComponent<PlateVerticalMover>();
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

        private Cell GetClosestDownPair(List<Cell> filtered, float movingPlateHeight) =>
            filtered.Where(cell => cell.Position.Height > movingPlateHeight)
                .OrderBy(cell => cell.Position.Height).Min();

        private Cell GetClosestUpPair(List<Cell> filtered, float movingPlateHeight) =>
            filtered.Where(cell => cell.Position.Height < movingPlateHeight)
                .OrderBy(cell => cell.Position.Height).Max();

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
                target.RemoveTwoPiFromAngle();

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
                target.AddTwoPiToAngle();

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