using CodeBase.EditorCells;
using CodeBase.Tools.Extension;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public int LevelId;
        public float Radius = 5.945f;
        public float ArchAngle = 22.5f;
        public float FloorHeight = 2f;
        public int Height = 0;

        [HideInInspector] public int Width = 16;
        [SerializeField] [SerializeReference] [HideInInspector]
        [FormerlySerializedAs("CellDataMap")] private CellData[] _cellDataMap;

        public int Size => Width * Height;
        public Vector3 HeroInitialPosition => GetPositionByCellType<InitialPlate>();
        public Vector3 FinishPosition => GetPositionByCellType<FinishPortal>();

        public CellData[] CellDataMap => _cellDataMap;

        private Vector3 GetPositionByCellType<T>()
        {
            for (int i = 0; i < Size; i++)
            {
                if (CellDataMap[i] is T == false)
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