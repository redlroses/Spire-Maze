using CodeBase.EditorCells;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        public float Radius = 5.945f;
        public float ArchAngle = 22.5f;
        public float FloorHeight = 2f;
        [HideInInspector] public int Width = 16;
        public int Height = 0;
        [SerializeField] [SerializeReference] public CellData[] CellDataMap;

        public int Size => Width * Height;
    }
}