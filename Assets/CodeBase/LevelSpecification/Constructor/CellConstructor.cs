using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;

namespace CodeBase.LevelSpecification.Constructor
{
    public class CellConstructor : ICellConstructor
    {
        private readonly Dictionary<Type, ICellConstructor> _constructors;

        public CellConstructor(GameStateMachine stateMachine,
            ISaveLoadService saveLoadService,
            IStaticDataService staticDataService,
            IRandomService randomService)
        {
            _constructors = new Dictionary<Type, ICellConstructor>
            {
                [typeof(Plate)] = new PlateConstructor(randomService),
                [typeof(Wall)] = new WallConstructor(randomService),
                [typeof(WallHole)] = new WallConstructor(randomService),
                [typeof(Key)] = new KeyConstructor(staticDataService),
                [typeof(Door)] = new DoorConstructor(),
                [typeof(MovingPlateMarker)] = new MovingPlateMarkerConstructor(),
                [typeof(Portal)] = new PortalConstructor(),
                [typeof(FinishPortal)] = new FinishPortalConstructor(stateMachine),
                [typeof(SpikeTrap)] = new SpikeTrapConstructor(),
                [typeof(FireTrap)] = new FireTrapConstructor(),
                [typeof(Rock)] = new RockConstructor(),
                [typeof(Savepoint)] = new SavepointConstructor(saveLoadService),
                [typeof(EnemySpawnPoint)] = new EnemySpawnPointConstructor(),
                [typeof(ItemSpawnPoint)] = new ItemSpawnPointConstructor(staticDataService),
            };
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell =>
            _constructors[typeof(TCell)].Construct<TCell>(gameFactory, cells);
    }
}