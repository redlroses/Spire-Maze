using System;
using CodeBase.Data;
using CodeBase.Level;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Tools
{
    [ExecuteAlways]
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private LevelStaticData _levelMapData;
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _defaultArc;
        [SerializeField] private GameObject _defaultWall;
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
            foreach (Transform plate in _parent)
            {
                DestroyImmediate(plate.gameObject);
            }
        }

        private void BuildLevel(LevelStaticData mapData)
        {
            float floor = 0;
            float arc = 0;
            int cellIndex = 0;

            for (int i = 0; i < mapData.Height; i++)
            {
                for (int j = 0; j < mapData.Width; j++)
                {
                    Cell(mapData.CellMap[cellIndex], new Vector3(0, floor, 0), Quaternion.Euler(new Vector3(0, arc, 0)));

                    if (CheckForMissedPlate(cellIndex))
                    {
                        Cell(CellType.Plate, new Vector3(0, floor + _floorHeight, 0), Quaternion.Euler(new Vector3(0, arc, 0)));
                    }

                    arc += _arcGrade;
                    cellIndex++;
                }

                floor += _floorHeight;
                arc = 0;
            }
        }

        private bool CheckForMissedPlate(int wallIndex)
        {
            int upperCellIndex = wallIndex + _levelMapData.Width;

            if (upperCellIndex >= _levelMapData.Size)
            {
                return false;
            }

            CellType upperCell = _levelMapData.CellMap[wallIndex + _levelMapData.Width];
            return upperCell == CellType.Air || upperCell == CellType.Wall;
        }

        private void Cell(CellType by, Vector3 at, Quaternion rotation)
        {
            switch (by)
            {
                case CellType.Air:
                    break;
                case CellType.Plate:
                    Instantiate(_defaultArc, at, rotation, _parent);
                    break;
                case CellType.Wall:
                    Instantiate(_defaultWall, at, rotation, _parent);
                    break;
                case CellType.Door:
                    break;
                case CellType.Key:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(by), by, null);
            }
        }
    }
}