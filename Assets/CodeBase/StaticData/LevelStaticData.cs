using CodeBase.Level;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        [HideInInspector] public int Width = 16;
        [HideInInspector] public int Height = 0;
        [HideInInspector] public CellType[] CellMap;

        public int Size => Width * Height;
    }
}