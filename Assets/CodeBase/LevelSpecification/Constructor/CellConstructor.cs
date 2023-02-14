using System;
using System.Collections.Generic;
using CodeBase.LevelSpecification.Cells;

namespace CodeBase.LevelSpecification.Constructor
{
    public class CellConstructor : ICellConstructor
    {
        private readonly Dictionary<Type, ICellConstructor> _constructors;

        public CellConstructor()
        {
            _constructors = new Dictionary<Type, ICellConstructor>
            {
                [typeof(Plate)] = new PlateConstructor(),
                [typeof(Wall)] = new WallConstructor(),
                [typeof(Key)] = new KeyConstructor(),
                [typeof(Door)] = new DoorConstructor(),
                [typeof(MovingPlate)] = new MovingPlateConstructor(),
                [typeof(MovingPlateMarker)] = new MovingPlateMarkerConstructor(),
            };
        }

        public void Construct<TCell>(Cell[] cells)
        {
            _constructors[typeof(TCell)].Construct<TCell>(cells);
        }
    }
}