using CodeBase.Level;
using UnityEngine;

namespace CodeBase.Data
{
    public readonly struct MapData
    {
        public readonly CellType[] Data;
        public readonly Vector2 Size;

        public MapData(CellType[] data)
        {
            Data = data;
            Size = new Vector2(16, 5);
        }
    }
}