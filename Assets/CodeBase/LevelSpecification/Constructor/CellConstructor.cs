using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.StaticData;

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
                [typeof(SpikeTrap)] = new SpikeTrapConstructor(),
                [typeof(FireTrap)] = new FireTrapConstructor(),
                [typeof(Rock)] = new RockConstructor(),
                [typeof(Savepoint)] = new SavepointConstructor(),
                [typeof(EnemySpawnPoint)] = new EnemySpawnPointConstructor(),
            };
        }

        public void Construct<TCell>(IGameFactory gameFactory, IStaticDataService staticData, Cell[] cells) where TCell : Cell
        {
            _constructors[typeof(TCell)].Construct<TCell>(gameFactory, staticData, cells);
        }
    }
}