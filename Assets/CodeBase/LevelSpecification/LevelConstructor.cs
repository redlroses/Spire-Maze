using System;
using System.Linq;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public class LevelConstructor : MonoBehaviour
    {
        [Space] [Header("Prefabs")]
        [SerializeField] private GameObject _defaultArc;
        [SerializeField] private GameObject _defaultWall;
        [SerializeField] private GameObject _defaultDoor;
        [SerializeField] private GameObject _defaultKey;
        [SerializeField] private GameObject _defaultMovingPlate;

        private CellConstructor _cellConstructor = new CellConstructor();

        public void Construct(Level level)
        {
            _cellConstructor.Construct<Plate>(level.Where(cell => cell.CellType == CellType.Plate).ToArray());
            _cellConstructor.Construct<Wall>(level.Where(cell => cell.CellType == CellType.Wall).ToArray());
            Debug.Log("Constructed");
        }

        private bool CheckForMissedPlate(Level level, int floorIndex, int wallIndex)
        {
            if (floorIndex > level.Height)
            {
                return false;
            }

            CellType upperCell = level.GetCell(floorIndex, wallIndex).CellType;
            return upperCell == CellType.Air || upperCell == CellType.Wall;
        }

        private void SpawnCell(Cell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Air:
                    break;
                case CellType.Plate:
                    Instantiate(_defaultArc, Vector3.zero, Quaternion.identity);
                    break;
                case CellType.Wall:
                    Instantiate(_defaultWall, Vector3.zero, Quaternion.identity);
                    break;
                case CellType.Door:
                    Instantiate(_defaultDoor, Vector3.zero, Quaternion.identity);
                    break;
                case CellType.Key:
                    Instantiate(_defaultKey, Vector3.zero, Quaternion.identity);
                    break;
                case CellType.MovingPlate:
                    Instantiate(_defaultMovingPlate, Vector3.zero, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cell.CellType), cell.CellType, null);
            }
        }
    }
}