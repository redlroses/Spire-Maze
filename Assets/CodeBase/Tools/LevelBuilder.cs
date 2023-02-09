using System;
using CodeBase.Level;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Tools
{
    [ExecuteAlways]
    public class LevelBuilder : MonoBehaviour
    {
        [Header("LevelData")]
        [SerializeField] private LevelStaticData _levelMapData;

        [Space] [Header("Prefabs")]
        [SerializeField] private GameObject _defaultArc;
        [SerializeField] private GameObject _defaultWall;
        [SerializeField] private GameObject _defaultDoor;
        [SerializeField] private GameObject _defaultKey;

        [Space] [Header("Settings")]
        [SerializeField] private Transform _levelContainer;

        [SerializeField] private float _arcGrade;
        [SerializeField] private float _floorHeight;


        [ContextMenu("Build")]
        private void Build()
        {
            Clear();
            BuildLevel(_levelMapData);
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
            int floor = 0;

            for (int i = 0; i < mapData.Height; i++)
            {
                BuildFloor(mapData, floor);
                floor++;
            }
        }

        private void BuildFloor(LevelStaticData mapData, int floor)
        {
            float arcGrade = 0;
            int cellIndex = _levelMapData.Width * floor;

            Transform floorParent = new GameObject($"Floor{floor}").transform;
            floorParent.parent = _levelContainer;

            for (int j = 0; j < mapData.Width; j++)
            {
                SpawnCell(mapData.CellMap[cellIndex], arcGrade, floorParent);

                // if (mapData.CellMap[cellIndex] == CellType.Wall && CheckForMissedPlate(cellIndex))
                // {
                //     SpawnCell(CellType.Plate, arcGrade, floorParent);
                // }

                arcGrade += _arcGrade;
                cellIndex++;
            }

            floorParent.transform.position = new Vector3(0, floor * _floorHeight, 0);
        }

        private bool CheckForMissedPlate(int wallIndex)
        {
            int upperCellIndex = wallIndex + _levelMapData.Width;

            if (upperCellIndex >= _levelMapData.Size)
            {
                return false;
            }

            CellType upperCell = _levelMapData.CellMap[upperCellIndex];
            return upperCell == CellType.Air || upperCell == CellType.Wall;
        }

        private void SpawnCell(CellType by, float grade, Transform parent)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, grade, 0));

            switch (by)
            {
                case CellType.Air:
                    break;
                case CellType.Plate:
                    Instantiate(_defaultArc, Vector3.zero, rotation, parent);
                    break;
                case CellType.Wall:
                    Instantiate(_defaultWall, Vector3.zero, rotation, parent);
                    break;
                case CellType.Door:
                    Instantiate(_defaultDoor, Vector3.zero, rotation, parent);
                    break;
                case CellType.Key:
                    Instantiate(_defaultKey, Vector3.zero, rotation, parent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(by), by, null);
            }
        }
    }
}