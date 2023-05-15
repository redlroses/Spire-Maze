using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.Pause;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Windows;

namespace CodeBase.LevelSpecification.Constructor
{
    public class CellConstructor : ICellConstructor
    {
        private readonly Dictionary<Type, ICellConstructor> _constructors;

        public CellConstructor(GameStateMachine stateMachine, ISaveLoadService saveLoadService, IStaticDataService staticDataService)
        {
            _constructors = new Dictionary<Type, ICellConstructor>
            {
                [typeof(Plate)] = new PlateConstructor(),
                [typeof(Wall)] = new WallConstructor(),
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
            };
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            _constructors[typeof(TCell)].Construct<TCell>(gameFactory, cells);
        }
    }
}