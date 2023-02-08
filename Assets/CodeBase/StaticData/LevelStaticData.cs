using CodeBase.Level;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        public int Width;
        public int Height;
        public CellType[] CellMap;

        public int Size => Width * Height;
    }
}