using System.Linq;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;
using CodeBase.MeshCombine;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using Door = CodeBase.LevelSpecification.Cells.Door;
using EnemySpawnPoint = CodeBase.LevelSpecification.Cells.EnemySpawnPoint;
using FinishPortal = CodeBase.LevelSpecification.Cells.FinishPortal;
using FireTrap = CodeBase.LevelSpecification.Cells.FireTrap;
using ItemSpawnPoint = CodeBase.LevelSpecification.Cells.ItemSpawnPoint;
using Key = CodeBase.LevelSpecification.Cells.Key;
using Plate = CodeBase.LevelSpecification.Cells.Plate;
using Portal = CodeBase.LevelSpecification.Cells.Portal;
using Rock = CodeBase.LevelSpecification.Cells.Rock;
using Savepoint = CodeBase.LevelSpecification.Cells.Savepoint;
using SpikeTrap = CodeBase.LevelSpecification.Cells.SpikeTrap;
using Wall = CodeBase.LevelSpecification.Cells.Wall;
using WallHole = CodeBase.LevelSpecification.Cells.WallHole;

namespace CodeBase.Services.LevelBuild
{
    public class LevelConstructor
    {
        private readonly CellConstructor _cellConstructor;
        private readonly IGameFactory _gameFactory;
        private readonly MeshCombiner _meshCombiner;

        public LevelConstructor(
            IGameFactory gameFactory,
            IStaticDataService staticData,
            GameStateMachine stateMachine,
            ISaveLoadService saveLoadService,
            IRandomService randomService)
        {
            _cellConstructor = new CellConstructor(stateMachine, saveLoadService, staticData, randomService);
            _gameFactory = gameFactory;
            _meshCombiner = new MeshCombiner();
        }

        public void Construct(Level level)
        {
            InitConstructor<Plate, EditorCells.Plate>(level);
            InitConstructor<Wall, EditorCells.Wall>(level);
            InitConstructor<WallHole, EditorCells.WallHole>(level);
            InitConstructor<Key, EditorCells.Key>(level);
            InitConstructor<Door, EditorCells.Door>(level);
            InitConstructor<MovingPlateMarker, MovingMarker>(level);
            InitConstructor<Portal, EditorCells.Portal>(level);
            InitConstructor<FinishPortal, EditorCells.FinishPortal>(level);
            InitConstructor<SpikeTrap, EditorCells.SpikeTrap>(level);
            InitConstructor<FireTrap, EditorCells.FireTrap>(level);
            InitConstructor<Rock, EditorCells.Rock>(level);
            InitConstructor<Savepoint, EditorCells.Savepoint>(level);
            InitConstructor<EnemySpawnPoint, EditorCells.EnemySpawnPoint>(level);
            InitConstructor<ItemSpawnPoint, EditorCells.ItemSpawnPoint>(level);
            CombineCells(level);
        }

        private void InitConstructor<TCell, TEditor>(Level level)
            where TCell : Cell
            where TEditor : CellData
        {
            _cellConstructor.Construct<TCell>(
                _gameFactory,
                level.Where(cell => cell.CellData is TEditor).ToArray());
        }

        private void CombineCells(Level level)
        {
            _meshCombiner.CombineAllColliders(level, _gameFactory.CreatePhysicMaterial(AssetPath.GroundMaterial));
            _meshCombiner.CombineAllMeshes(level.Origin, _gameFactory.CreateMaterial(AssetPath.SpireMaterial));
        }
    }
}