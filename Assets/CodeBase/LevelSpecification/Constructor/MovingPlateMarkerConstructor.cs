﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic;
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
        private Spire _spire;
        private Cell[] _markers;
        private Cell[] _movingPlates;
        private IGameFactory _gameFactory;

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            if (cells.Any() == false)
                return;

            _gameFactory = gameFactory;
            _spire = cells[0].Container.root.GetComponentInChildren<Spire>();
            _markers = cells;
            _movingPlates = cells.Where(cell => ((MovingMarker)cell.CellData).IsLiftHolder).ToArray();

            foreach (Cell cell in _movingPlates)
                gameFactory.CreateCell<MovingPlate>(cell.Container);

            foreach (Cell cell in _markers)
                gameFactory.CreateCell<MovingPlateMarker>(cell.Container);

            foreach (Cell fromCell in _movingPlates)
            {
                Cell toCell = FindPair(fromCell);
                CreateRails(fromCell, toCell);
                ApplyDestinationMarkers(fromCell, toCell);
            }
        }

        private void CreateRails(Cell initialCell, Cell finishCell)
        {
            MovingMarker initialMarker = (MovingMarker)initialCell.CellData;

            if (IsHorizontalRail(initialMarker))
            {
                Cell leftCell = ChooseCellByDirection(initialCell, finishCell, PlateMoveDirection.Right);
                Cell rightCell = ChooseCellByDirection(initialCell, finishCell, PlateMoveDirection.Left);
                Cell currentCell = _spire.GetLeftFrom(rightCell);

                do
                {
                    _gameFactory.CreateHorizontalRail(currentCell.Container);
                    currentCell = _spire.GetLeftFrom(currentCell);
                } while (currentCell.Id != leftCell.Id);

                _gameFactory.CreateHorizontalRailLock(leftCell.Container)
                    .transform.GetChild(0).transform.Rotate(Trigonometry.PiGrade, 0, 0);
                _gameFactory.CreateHorizontalRailLock(rightCell.Container);
            }
            else
            {
                Cell upCell = ChooseCellByDirection(initialCell, finishCell, PlateMoveDirection.Down);
                Cell downCell = ChooseCellByDirection(initialCell, finishCell, PlateMoveDirection.Up);
                Cell currentCell = _spire.GetUpFrom(downCell);

                while (currentCell.Id != upCell.Id)
                {
                    _gameFactory.CreateVerticalRail(currentCell.Container);
                    currentCell = _spire.GetUpFrom(currentCell);
                }

                _gameFactory.CreateVerticalRailLock(upCell.Container);
                _gameFactory.CreateVerticalRailLock(downCell.Container)
                    .transform.GetChild(0).transform.Rotate(Trigonometry.PiGrade, 0, 0);
            }
        }

        private bool IsHorizontalRail(MovingMarker initialMarker) =>
            initialMarker.Direction is PlateMoveDirection.Left or PlateMoveDirection.Right;

        private Cell ChooseCellByDirection(Cell initialCell, Cell finishCell, PlateMoveDirection plateMoveDirection) =>
            ((MovingMarker)initialCell.CellData).Direction == plateMoveDirection ? initialCell : finishCell;

        private void ApplyDestinationMarkers(Cell fromCell, Cell toCell)
        {
            LiftDestinationMarker initialMarker = fromCell.Container.GetComponentInChildren<LiftDestinationMarker>();
            LiftDestinationMarker destinationMarker = toCell.Container.GetComponentInChildren<LiftDestinationMarker>();
            LiftPlate liftPlate = fromCell.Container.GetComponentInChildren<LiftPlate>();

            initialMarker.Construct(fromCell.Position);
            destinationMarker.Construct(toCell.Position);

            PlateMoveDirection plateMoveDirection = ((MovingMarker)fromCell.CellData).Direction;
            IPlateMover mover = ChooseMoverComponent(liftPlate, plateMoveDirection);

            liftPlate.Construct(initialMarker, destinationMarker, mover, plateMoveDirection);
        }

        private IPlateMover ChooseMoverComponent(LiftPlate liftPlate, PlateMoveDirection direction)
        {
            if (direction is PlateMoveDirection.Left or PlateMoveDirection.Right)
            {
                Object.Destroy(liftPlate.gameObject.GetComponent<PlateVerticalMover>());
                return liftPlate.gameObject.GetComponent<PlateHorizontalMover>();
            }

            Object.Destroy(liftPlate.gameObject.GetComponent<PlateHorizontalMover>());
            return liftPlate.gameObject.GetComponent<PlateVerticalMover>();
        }

        private Cell FindPair(Cell movingPlateCell)
        {
            PlateMoveDirection plateMoveDirection = ((MovingMarker)movingPlateCell.CellData).Direction;
            return plateMoveDirection is PlateMoveDirection.Left or PlateMoveDirection.Right
                ? FindHorizontalPair(movingPlateCell)
                : FindVerticalPair(movingPlateCell);
        }

        private Cell FindVerticalPair(Cell movingPlateCell)
        {
            float movingPlateHeight = movingPlateCell.Position.Height;

            if (((MovingMarker)movingPlateCell.CellData).Direction == PlateMoveDirection.Up)
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

            if (((MovingMarker)movingPlateCell.CellData).Direction == PlateMoveDirection.Left)
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
                    break;

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
                    break;

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
                    ((MovingMarker)cell.CellData).Direction == type).ToList();
            return filtered;
        }

        private List<Cell> GetFilteredByAngle(Cell movingPlateCell, PlateMoveDirection type)
        {
            List<Cell> filtered =
                _markers.Where(cell =>
                    cell.Position.Angle.EqualsApproximately(movingPlateCell.Position.Angle) &&
                    ((MovingMarker)cell.CellData).Direction == type).ToList();
            return filtered;
        }
    }
}