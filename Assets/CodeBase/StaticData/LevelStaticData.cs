using CodeBase.Data.Cell;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        [HideInInspector] public int Width = 16;
        public int Height = 0;
        [SerializeField] [SerializeReference] public CellData[] CellDataMap;

        public int Size => Width * Height;
    }
}