using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.FileRead;
using CodeBase.Level;
using UnityEngine;

namespace CodeBase.Tools
{
    [ExecuteInEditMode]
    public class LevelEditor : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _defaultArc;
        [SerializeField] private float _arcGrade;
        [SerializeField] private float _floorHeight;

        private MapReader _mapReader = new MapReader();
        private MapData _mapData;

        private MapData GetMapData() =>
            _mapReader.GetMapData(1);

        [ContextMenu("Build")]
        private void Build()
        {
            Clear();
            _mapData = GetMapData();
            BuildLevel(_mapData);
        }

        [ContextMenu("Clear")]
        private void Clear()
        {
            foreach (Transform plate in _parent)
            {
                DestroyImmediate(plate.gameObject);
            }
        }

        private void BuildLevel(MapData mapData)
        {
            float floor = 0;
            float arc = 0;
            int cellIndex = 0;

            for (int i = 0; i < mapData.Size.y; i++)
            {
                for (int j = 0; j < mapData.Size.x; j++)
                {
                    if (mapData.Data[cellIndex] == CellType.Plate)
                    {
                        Instantiate(_defaultArc, new Vector3(0, floor, 0), Quaternion.Euler(new Vector3(0, arc, 0)), _parent);
                    }

                    arc += _arcGrade;
                    cellIndex++;
                }

                floor += _floorHeight;
                arc = 0;
            }
        }
    }
}