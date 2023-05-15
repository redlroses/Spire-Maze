﻿using System;
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

            foreach (var cell in _movingPlates)
            {
                gameFactory.CreateCell<MovingPlate>(cell.Container);
            }

            foreach (var cell in _markers)
            {
                gameFactory.CreateCell<MovingPlateMarker>(cell.Container);
            }

            foreach (var movingPlates in _movingPlates)
            {
                ApplyMovingPlate(movingPlates);
            }
        }

        private void ApplyMovingPlate(Cell movingPlate)
        {
            var pairMarker = FindPair(movingPlate);
            var initialMarker = movingPlate.Container.GetComponentInChildren<LiftDestinationMarker>();
            var destinationMarker = pairMarker.Container.GetComponentInChildren<LiftDestinationMarker>();
            var liftPlate = movingPlate.Container.GetComponentInChildren<LiftPlate>();

            initialMarker.Construct(movingPlate.Position);
            destinationMarker.Construct(pairMarker.Position);

            var plateMoveDirection = ((MovingMarker) movingPlate.CellData).Direction;
            var mover = ChooseMoverComponent(liftPlate, plateMoveDirection);

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
            var plateMoveDirection = ((MovingMarker) movingPlateCell.CellData).Direction;
            return plateMoveDirection == PlateMoveDirection.Left || plateMoveDirection == PlateMoveDirection.Right
                ? FindHorizontalPair(movingPlateCell)
                : FindVerticalPair(movingPlateCell);
        }

        private Cell FindVerticalPair(Cell movingPlateCell)
        {
            float movingPlateHeight = movingPlateCell.Position.Height;

            if (((MovingMarker) movingPlateCell.CellData).Direction == PlateMoveDirection.Up)
            {
                var filtered = GetFilteredByAngle(movingPlateCell, PlateMoveDirection.Down);
                return GetClosestDownPair(filtered, movingPlateHeight);
            }
            else
            {
                var filtered = GetFilteredByAngle(movingPlateCell, PlateMoveDirection.Up);
                return GetClosestUpPair(filtered, movingPlateHeight);
            }
        }

        private Cell FindHorizontalPair(Cell movingPlateCell)
        {
            float movingPlateAngle = movingPlateCell.Position.Angle;

            if (((MovingMarker) movingPlateCell.CellData).Direction == PlateMoveDirection.Left)
            {
                var filtered = GetFilteredByHeight(movingPlateCell, PlateMoveDirection.Right);
                return GetClosestRightPair(filtered, movingPlateAngle);
            }
            else
            {
                var filtered = GetFilteredByHeight(movingPlateCell, PlateMoveDirection.Left);
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
            var isIncreaseAngle = false;

            for (var i = 0; i <= 1; i++)
            {
                possibleMarkers = filtered.Where(cell =>
                    cell.Position.Angle - Trigonometry.TwoPiGrade * i - movingPlateAngle < 0).ToList();

                if (possibleMarkers.Any())
                {
                    break;
                }

                isIncreaseAngle = true;
            }

            var target = (possibleMarkers ?? throw new ArgumentNullException(nameof(possibleMarkers)))
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
            var isIncreaseAngle = false;

            for (var i = 0; i <= 1; i++)
            {
                possibleMarkers = filtered.Where(cell =>
                    cell.Position.Angle + Trigonometry.TwoPiGrade * i - movingPlateAngle > 0).ToList();

                if (possibleMarkers.Any())
                {
                    break;
                }

                isIncreaseAngle = true;
            }

            var target = (possibleMarkers ?? throw new ArgumentNullException(nameof(possibleMarkers)))
                .OrderBy(cell => cell.Position.Angle).First();

            if (isIncreaseAngle)
            {
                target.AddTwoPiToAngle();
            }

            return target;
        }

        private List<Cell> GetFilteredByHeight(Cell movingPlateCell, PlateMoveDirection type)
        {
            var filtered =
                _markers.Where(cell =>
                    cell.Position.Height.EqualsApproximately(movingPlateCell.Position.Height) &&
                    ((MovingMarker) cell.CellData).Direction == type).ToList();
            return filtered;
        }

        private List<Cell> GetFilteredByAngle(Cell movingPlateCell, PlateMoveDirection type)
        {
            var filtered =
                _markers.Where(cell =>
                    cell.Position.Angle.EqualsApproximately(movingPlateCell.Position.Angle) &&
                    ((MovingMarker) cell.CellData).Direction == type).ToList();
            return filtered;
        }
    }
}