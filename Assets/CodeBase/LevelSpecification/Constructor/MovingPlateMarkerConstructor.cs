using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Lift;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Tools.Constants;
using CodeBase.Tools.Extension;
using UnityEngine;

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

            Debug.Log($"MovingPlate position: height {_movingPlates.First().CellPosition.Height}, angle {_movingPlates.First().CellPosition.Angle}");

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

            initialMarker.Construct(movingPlate.CellPosition, _directions[movingPlate.CellType & CellType.MovingMarker]);
            destinationMarker.Construct(pairMarker.CellPosition, _directions[pairMarker.CellType & CellType.MovingMarker]);

            IPlateMover mover = movingPlate.IsTypeOf(CellType.Left | CellType.Right)
                ? (IPlateMover) liftPlate.gameObject.AddComponent<PlateHorizontalMover>()
                : liftPlate.gameObject.AddComponent<PlateVerticalMover>();

            liftPlate.Construct(initialMarker, destinationMarker, mover);
        }

        private Cell FindPair(Cell movingPlateCell)
        {
            float movingPlateAngle = movingPlateCell.CellPosition.Angle;
            bool isIncreaseAngle = false;
            Cell target = null;

            if ((movingPlateCell.CellType & CellType.Left) != 0)
            {
                List<Cell> filtered =
                    _markers.Where(cell =>
                        cell.CellPosition.Height.EqualsApproximately(movingPlateCell.CellPosition.Height) &&
                        cell.IsTypeOf(CellType.Right)).ToList();

                List<Cell> possibleMarkers = null;

                for (int i = 0; i <= 1; i++)
                {
                    possibleMarkers = filtered.Where(cell =>
                        cell.CellPosition.Angle + Trigonometry.TwoPiGrade * i - movingPlateAngle > 0).ToList();

                    if (possibleMarkers.Any())
                    {
                        break;
                    }

                    isIncreaseAngle = true;
                }

                target = (possibleMarkers ?? throw new ArgumentNullException(nameof(possibleMarkers)))
                    .OrderBy(cell => cell.CellPosition.Angle).First();

                Debug.Log(target.CellPosition.Angle);
                if (isIncreaseAngle)
                {
                    target.AddTwoPiToAngle();
                }
                Debug.Log(target.CellPosition.Angle);

                Debug.Log($"Marker position: height {target.CellPosition.Height}, angle {target.CellPosition.Angle}");
            }

            return target;
        }
    }
}