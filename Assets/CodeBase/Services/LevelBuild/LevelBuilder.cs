using System.Linq;
using CodeBase.Data.Cell;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.LevelBuild
{
    public class LevelBuilder : ILevelBuilder
    {
        private readonly LevelConstructor _levelConstructor;
        private readonly IGameFactory _gameFactory;

        private Transform _levelContainer;
        private float _radius;
        private float _archAngle;
        private float _floorHeight;
        private Level _level;

        public LevelBuilder(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _levelConstructor = new LevelConstructor();
        }

        public Level Build(LevelStaticData levelStaticData)
        {
            _levelContainer = _gameFactory.CreateSpire().transform;
            SetUpLevelParameters(levelStaticData);
            BuildLevel(levelStaticData);
            _level.SelfTransform = _levelContainer;
            _levelConstructor.Construct(_gameFactory, _level);
            return _level;
        }

        public void Clear()
        {
            _levelContainer.gameObject.AddComponent<Destroyer>();
        }

        private void BuildLevel(LevelStaticData mapData)
        {
            _level = new Level(mapData.Height, _levelContainer);

            for (int i = mapData.Height - 1; i >= 0; i--)
            {
                Vector3 containerPosition = new Vector3(0, i * _floorHeight, 0);
                Transform floorContainer = CreateContainer($"Floor {i + 1}", _levelContainer, containerPosition);
                floorContainer.gameObject.AddComponent<MeshFilter>();
                floorContainer.gameObject.AddComponent<MeshRenderer>();
                floorContainer.gameObject.AddComponent<MeshCollider>();
                CellData[] floorCells = mapData.CellDataMap.Skip(i * mapData.Width).Take(mapData.Width).ToArray();
                Floor floor = BuildFloor(floorCells, floorContainer);
                _level.Add(floor);
            }
        }

        private void SetUpLevelParameters(LevelStaticData levelStaticData)
        {
            _radius = levelStaticData.Radius;
            _archAngle = levelStaticData.ArchAngle;
            _floorHeight = levelStaticData.FloorHeight;
        }

        private Floor BuildFloor(CellData[] floorCells, Transform container)
        {
            Floor floor = new Floor(floorCells.Length, container);

            for (int i = 0; i < floorCells.Length; i++)
            {
                Vector3 containerPosition = GetPosition(i * _archAngle, _radius);
                Quaternion containerRotation = GetRotation(i * _archAngle);
                Transform cellContainer = CreateContainer($"Cell {i + 1}: {floorCells[i]}", container,
                    containerPosition, containerRotation);
                Cell cell = new Cell(floorCells[i], cellContainer);
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
    }
}