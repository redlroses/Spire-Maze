using CodeBase.LevelSpecification;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        public int Width = 16;
        public int Height = 0;
        public CellType[] CellMap;

        public int Size => Width * Height;
    }
}