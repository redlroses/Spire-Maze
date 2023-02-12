using System;
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

        public void Construct(Level level)
        {
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
                case CellType.MovingPlate:
                    Instantiate(_defaultMovingPlate, Vector3.zero, rotation, parent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(by), by, null);
            }
        }
    }
}