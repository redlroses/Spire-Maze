using CodeBase.Level;

namespace CodeBase.Data
{
    public struct LevelMapData
    {
        public string Key;
        public int Width;
        public int Height;
        public CellType[] Map;

        public LevelMapData(string key, int width, int height, CellType[] map)
        {
            Key = key;
            Width = width;
            Height = height;
            Map = map;
        }
    }
}
