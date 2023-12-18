using System.Collections.Generic;
using CodeBase.EditorCells;
using CodeBase.Tools.Extension;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [FormerlySerializedAs("LevelId")] [SerializeField] private int _levelId;
        [FormerlySerializedAs("Radius")] [SerializeField] private float _radius = 5.945f;
        [FormerlySerializedAs("ArchAngle")] [SerializeField] private float _archAngle = 22.5f;
        [FormerlySerializedAs("FloorHeight")] [SerializeField] private float _floorHeight = 2f;
        [FormerlySerializedAs("Height")] [SerializeField] private int _height;
        [FormerlySerializedAs("Width")] [SerializeField] private int _width = 16;
        [SerializeField] [SerializeReference] [HideInInspector]
        [FormerlySerializedAs("CellDataMap")] private CellData[] _cellDataMap;

        public int Size => Width * Height;

        public Vector3 HeroInitialPosition => GetPositionByCellType<InitialPlate>();

        public Vector3 FinishPosition => GetPositionByCellType<FinishPortal>();

        public IReadOnlyCollection<CellData> CellDataMap => _cellDataMap;

        public int LevelId => _levelId;

        public float Radius => _radius;

        public float ArchAngle => _archAngle;

        public float FloorHeight => _floorHeight;

        public int Height => _height;

        public int Width => _width;

        private Vector3 GetPositionByCellType<T>()
        {
            for (int i = 0; i < Size; i++)
            {
                if (_cellDataMap[i] is T == false)
                {
                    continue;
                }

                float height = i / Width * FloorHeight;
                float angle = i % Width * ArchAngle;
                return GetPosition(angle, Radius).ChangeY(height);
            }

            return Vector3.zero;
        }

        private Vector3 GetPosition(float byArcGrade, float radius)
        {
            float posX = Mathf.Cos(byArcGrade * Mathf.Deg2Rad) * radius;
            float posZ = Mathf.Sin(byArcGrade * Mathf.Deg2Rad) * radius;
            return new Vector3(posX, 0, posZ);
        }
    }
}