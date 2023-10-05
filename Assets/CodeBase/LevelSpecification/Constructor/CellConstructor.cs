﻿using System;
using System.Collections.Generic;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using Door = CodeBase.LevelSpecification.Cells.Door;
using EnemySpawnPoint = CodeBase.LevelSpecification.Cells.EnemySpawnPoint;
using ItemSpawnPoint = CodeBase.LevelSpecification.Cells.ItemSpawnPoint;
using FinishPortal = CodeBase.LevelSpecification.Cells.FinishPortal;
using FireTrap = CodeBase.LevelSpecification.Cells.FireTrap;
using Key = CodeBase.LevelSpecification.Cells.Key;
using Plate = CodeBase.LevelSpecification.Cells.Plate;
using Portal = CodeBase.LevelSpecification.Cells.Portal;
using Rock = CodeBase.LevelSpecification.Cells.Rock;
using Savepoint = CodeBase.LevelSpecification.Cells.Savepoint;
using SpikeTrap = CodeBase.LevelSpecification.Cells.SpikeTrap;
using Wall = CodeBase.LevelSpecification.Cells.Wall;
using Wall2 = CodeBase.LevelSpecification.Cells.Wall2;

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
                [typeof(Wall2)] = new WallConstructor(),
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

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            _constructors[typeof(TCell)].Construct<TCell>(gameFactory, cells);
        }
    }
}