using CodeBase.Extensions;

namespace CodeBase.FileRead
{
    public class MapReader : Reader
    {
        private const string LevelMapDefaultPath = "MapFiles/";
        private const string LevelName = "Level";

        public MapData GetMapData(int levelIndex)
        {
            string levelPath = $"{LevelMapDefaultPath}{LevelName}{levelIndex}";
            return ReadText(levelPath).ToMapData();
        }
    }
}