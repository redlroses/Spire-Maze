using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.Tools.Constants;
using CodeBase.Tools.Extension;
using UnityEngine;
using FinishPortal = CodeBase.EditorCells.FinishPortal;

namespace CodeBase.Services.LevelBuild
{
    public class LevelBuilder : ILevelBuilder
    {
        private const string ConstructException = "You must build the level before constructing it";

        private readonly int _additionalSpireSegmentsCount = 2;

        private readonly LevelConstructor _levelConstructor;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IRandomService _randomService;

        private Transform _levelContainer;
        private Level _level;
        private LevelStaticData _levelStaticData;

        private float FloorHeight => _levelStaticData.FloorHeight;
        private float Radius => _levelStaticData.Radius;
        private float ArchAngle => _levelStaticData.ArchAngle;

        public LevelBuilder(IGameFactory gameFactory, IStaticDataService staticData, GameStateMachine stateMachine,
            ISaveLoadService saveLoadService, IPersistentProgressService persistentProgressService, IRandomService randomService)
        {
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;
            _randomService = randomService;
            _levelConstructor = new LevelConstructor(gameFactory, staticData, stateMachine, saveLoadService);
        }

        public Level Build(LevelStaticData levelStaticData)
        {
            _levelStaticData = levelStaticData;
            _levelContainer = CreateSpire(levelStaticData);
            CreateLevel(levelStaticData);
            _level.SelfTransform = _levelContainer;
            _persistentProgressService.Progress.WorldData.LevelPositions.FinishPosition = GetFinishPosition();
            _persistentProgressService.TemporalProgress.LevelHeightRange = new Vector2(0, FloorHeight * _level.Height);

            return _level;
        }

        public void Construct()
        {
            if (_level == null)
            {
                throw new Exception(ConstructException);
            }

            _levelConstructor.Construct(_level);
        }

        public void Clear()
        {
            _level = default;
            _levelContainer = default;
            _levelStaticData = default;
        }

        private void CreateLevel(LevelStaticData mapData)
        {
            _level = new Level(mapData.Height, _levelContainer);

            for (int i = mapData.Height - 1; i >= 0; i--)
            {
                Vector3 containerPosition = new Vector3(0, i * FloorHeight, 0);
                Transform floorContainer = CreateContainer($"Floor {i + 1}", _levelContainer, containerPosition);
                CellData[] floorCells = mapData.CellDataMap.Skip(i * mapData.Width).Take(mapData.Width).ToArray();
                Floor floor = BuildFloor(floorCells, floorContainer, i);
                _level.Add(floor);
            }
        }

        private Floor BuildFloor(CellData[] floorCells, Transform container, int floorIndex)
        {
            Floor floor = new Floor(floorCells.Length, container);

            for (int i = 0; i < floorCells.Length; i++)
            {
                Vector3 containerPosition = GetPosition(i * ArchAngle, Radius);
                Quaternion containerRotation = GetRotation(i * ArchAngle);
                Transform cellContainer = CreateContainer($"Cell {i + 1}: {floorCells[i]}", container,
                    containerPosition, containerRotation);
                int cellId = floorIndex * (floorCells.Length + 1) + i;
                Cell cell = new Cell(floorCells[i], cellContainer, cellId);
                floor.Add(cell);
            }

            return floor;
        }

        private Transform CreateSpire(LevelStaticData levelStaticData)
        {
            Transform spireRoot = new GameObject("Spire").transform;

            int segmentCount = Mathf.RoundToInt(levelStaticData.Height * Arithmetic.Half);

            for (int i = 0 - _additionalSpireSegmentsCount; i < segmentCount + _additionalSpireSegmentsCount; i++)
            {
                Vector3 position = Vector3.zero.ChangeY(i * levelStaticData.FloorHeight / Arithmetic.Half);
                Quaternion rotation = Quaternion.Euler(0, _randomService.Range(0f, Trigonometry.TwoPiGrade), 0f);
                GameObject segment = _gameFactory.CreateSpireSegment(position, rotation);
                segment.transform.parent = spireRoot;
            }

            return spireRoot;
        }

        private Transform CreateContainer(string name, Transform parent, Vector3 position)
        {
            Transform floorContainer = new GameObject(name).transform;
            floorContainer.parent = parent;
            floorContainer.localPosition = position;
            return floorContainer;
        }

        private Transform CreateContainer(string name, Transform parent, Vector3 position, Quaternion rotation)
        {
            Transform floorContainer = new GameObject(name).transform;
            floorContainer.parent = parent;
            floorContainer.SetLocalPositionAndRotation(position, rotation);
            return floorContainer;
        }

        private Quaternion GetRotation(float grade) =>
            Quaternion.Euler(0, -grade, 0);

        private Vector3 GetPosition(float byArcGrade, float radius)
        {
            float posX = Mathf.Cos(byArcGrade * Mathf.Deg2Rad) * radius;
            float posZ = Mathf.Sin(byArcGrade * Mathf.Deg2Rad) * radius;
            return new Vector3(posX, 0, posZ);
        }

        private Vector3Data GetFinishPosition()
        {
            Cell cell = _level.FirstOrDefault(cell => cell.CellData is FinishPortal);
            return cell is null ? new Vector3Data() : cell.Container.position.AsVectorData();
        }
    }
}