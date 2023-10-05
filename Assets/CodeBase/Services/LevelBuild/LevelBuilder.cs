using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;
using UnityEngine;
using FinishPortal = CodeBase.EditorCells.FinishPortal;

namespace CodeBase.Services.LevelBuild
{
    public class LevelBuilder : ILevelBuilder
    {
        private readonly LevelConstructor _levelConstructor;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _persistentProgressService;

        private Transform _levelContainer;
        private float _radius;
        private float _archAngle;
        private float _floorHeight;
        private Level _level;

        public LevelBuilder(IGameFactory gameFactory, IStaticDataService staticData, GameStateMachine stateMachine,
            ISaveLoadService saveLoadService, IPersistentProgressService persistentProgressService)
        {
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;
            _levelConstructor = new LevelConstructor(gameFactory, staticData, stateMachine, saveLoadService);
        }

        public Level Build(LevelStaticData levelStaticData)
        {
            _levelContainer = _gameFactory.CreateSpire().transform;
            SetUpLevelParameters(levelStaticData);
            BuildLevel(levelStaticData);
            _level.SelfTransform = _levelContainer;
            _persistentProgressService.Progress.WorldData.LevelPositions.FinishPosition = GetFinishPosition();

            Debug.Log($"Update finish position: {_persistentProgressService.Progress.WorldData.LevelPositions.FinishPosition.AsUnityVector()}");

            return _level;
        }

        public void Construct()
        {
            if (_level == null)
            {
                throw new Exception("You must build the level before constructing it");
            }

            _levelConstructor.Construct(_level);
        }

        public void Clear()
        {
            _level = null;
            _radius = default;
            _archAngle = default;
            _floorHeight = default;
            _levelContainer = null;
        }

        private void BuildLevel(LevelStaticData mapData)
        {
            _level = new Level(mapData.Height, _levelContainer);

            for (int i = mapData.Height - 1; i >= 0; i--)
            {
                Vector3 containerPosition = new Vector3(0, i * _floorHeight, 0);
                Transform floorContainer = CreateContainer($"Floor {i + 1}", _levelContainer, containerPosition);
                CellData[] floorCells = mapData.CellDataMap.Skip(i * mapData.Width).Take(mapData.Width).ToArray();
                Floor floor = BuildFloor(floorCells, floorContainer, i);
                _level.Add(floor);
            }
        }

        private void SetUpLevelParameters(LevelStaticData levelStaticData)
        {
            _radius = levelStaticData.Radius;
            _archAngle = levelStaticData.ArchAngle;
            _floorHeight = levelStaticData.FloorHeight;
        }

        private Floor BuildFloor(CellData[] floorCells, Transform container, int floorIndex)
        {
            Floor floor = new Floor(floorCells.Length, container);

            for (int i = 0; i < floorCells.Length; i++)
            {
                Vector3 containerPosition = GetPosition(i * _archAngle, _radius);
                Quaternion containerRotation = GetRotation(i * _archAngle);
                Transform cellContainer = CreateContainer($"Cell {i + 1}: {floorCells[i]}", container,
                    containerPosition, containerRotation);
                int cellId = floorIndex * (floorCells.Length + 1) + i;
                Cell cell = new Cell(floorCells[i], cellContainer, cellId);
                floor.Add(cell);
            }

            return floor;
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

        private Vector3Data GetFinishPosition() =>
            _level.First(cell => cell.CellData is FinishPortal).Container.position.AsVectorData();
    }
}