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
using WallHole = CodeBase.LevelSpecification.Cells.WallHole;

namespace CodeBase.Services.LevelBuild
{
    public class LevelConstructor
    {
        private readonly IGameFactory _gameFactory;
        private readonly CellConstructor _cellConstructor;
        private readonly MeshCombiner _meshCombiner;

        public LevelConstructor(IGameFactory gameFactory,
            IStaticDataService staticData,
            GameStateMachine stateMachine, ISaveLoadService saveLoadService, IRandomService randomService)
        {
            _cellConstructor = new CellConstructor(stateMachine, saveLoadService, staticData, randomService);
            _gameFactory = gameFactory;
            _meshCombiner = new MeshCombiner();
        }

        public void Construct(Level level)
        {
            _cellConstructor.Construct<Plate>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Plate).ToArray());
            _cellConstructor.Construct<Wall>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Wall).ToArray());
            _cellConstructor.Construct<WallHole>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.WallHole).ToArray());
            _cellConstructor.Construct<Key>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Key).ToArray());
            _cellConstructor.Construct<Door>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Door).ToArray());
            _cellConstructor.Construct<MovingPlateMarker>(_gameFactory,
                level.Where(cell => cell.CellData is MovingMarker).ToArray());
            _cellConstructor.Construct<Portal>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Portal).ToArray());
            _cellConstructor.Construct<FinishPortal>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.FinishPortal).ToArray());
            _cellConstructor.Construct<SpikeTrap>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.SpikeTrap).ToArray());
            _cellConstructor.Construct<FireTrap>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.FireTrap).ToArray());
            _cellConstructor.Construct<Rock>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Rock).ToArray());
            _cellConstructor.Construct<Savepoint>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Savepoint).ToArray());
            _cellConstructor.Construct<EnemySpawnPoint>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.EnemySpawnPoint).ToArray());
            _cellConstructor.Construct<ItemSpawnPoint>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.ItemSpawnPoint).ToArray());
            CombineCells(level);
        }

        private void CombineCells(Level level)
        {
            _meshCombiner.CombineAllColliders(level, _gameFactory.CreatePhysicMaterial(AssetPath.GroundMaterial));
            _meshCombiner.CombineAllMeshes(level.Origin, _gameFactory.CreateMaterial(AssetPath.SpireMaterial));
        }
    }
}