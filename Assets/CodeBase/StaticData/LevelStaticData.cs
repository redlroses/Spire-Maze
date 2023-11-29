using CodeBase.EditorCells;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Tools.Extension;
using UnityEngine;
using FinishPortal = CodeBase.EditorCells.FinishPortal;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelName;
        public int LevelId;
        public float Radius = 5.945f;
        public float ArchAngle = 22.5f;
        public float FloorHeight = 2f;
        [HideInInspector] public int Width = 16;
        public int Height = 0;
        [SerializeField] [SerializeReference] public CellData[] CellDataMap;

        public int Size => Width * Height;
        public Cell HeroCell;
        public Cell FinishCell;
        public Vector3 HeroInitialPosition => HeroCell.Container.position;
        public Vector3 FinishPosition  => FinishCell.Container.position;

        private Vector3 GetPositionByCellType<T>()
        {
            for (int i = 0; i < Size; i++)
            {
                if (CellDataMap[i] is T == false)
                {
                    continue;
                }
            
                float height = i / (float) Width * FloorHeight;
                Debug.Log($"Type: {typeof(T)} - i {i}; Size {Size}; height {height}");
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