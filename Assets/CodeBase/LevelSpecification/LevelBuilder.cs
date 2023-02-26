using System.Linq;
using CodeBase.Data.Cell;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    [RequireComponent(typeof(LevelConstructor))]
    [ExecuteAlways]
    public class LevelBuilder : MonoBehaviour
    {
        [Header("LevelData")] [SerializeField] private LevelStaticData _levelMapData;

        [Space] [Header("Settings")]
        [SerializeField] private Transform _levelContainer;
        [SerializeField] private float _radius;
        [SerializeField] private float _arcGrade;
        [SerializeField] private float _floorHeight;

        private LevelConstructor _levelConstructor;
        private Level _level;

        private void Awake()
        {
            Build();
        }

        [ContextMenu("Build")]
        private void Build()
        {
            Clear();
            BuildLevel(_levelMapData);
            _levelConstructor ??= GetComponent<LevelConstructor>();
            _levelConstructor.Construct(AllServices.Container.Single<IGameFactory>(), _level);
        }

        [ContextMenu("Clear")]
        private void Clear()
        {
            for (int i = 0; i < 100; i++)
            {
                foreach (Transform plate in _levelContainer)
                {
                    DestroyImmediate(plate.gameObject);
                }
            }
        }

        private void BuildLevel(LevelStaticData mapData)

        {
            _level = new Level(mapData.Height, _levelContainer);

            for (int i = mapData.Height - 1; i >= 0; i--)
            {
                Vector3 containerPosition = new Vector3(0, i * _floorHeight, 0);
                Transform floorContainer = CreateContainer($"Floor {i + 1}", _levelContainer, containerPosition);
                CellData[] floorCells = mapData.CellDataMap.Skip(i * mapData.Width).Take(mapData.Width).ToArray();
                Floor floor = BuildFloor(floorCells, floorContainer);
                _level.Add(floor);
            }
        }

        private Floor BuildFloor(CellData[] floorCells, Transform container)
        {
            Floor floor = new Floor(floorCells.Length, container);

            for (int i = 0; i < floorCells.Length; i++)
            {
                Vector3 containerPosition = GetPosition(i * _arcGrade, _radius);
                Quaternion containerRotation = GetRotation(i * _arcGrade);
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