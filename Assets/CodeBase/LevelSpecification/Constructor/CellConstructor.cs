using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
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
                [typeof(MovingPlateMarker)] = new MovingPlateMarkerConstructor(),
                [typeof(Portal)] = new PortalConstructor(),
            };
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
        {
            _constructors[typeof(TCell)].Construct<TCell>(gameFactory, cells);
        }
    }
}