using CodeBase.Data;
using CodeBase.Tools;

namespace CodeBase.Infrastructure.FileRead
{
    public class MapReader : Reader
    {
        private const string LevelMapDefaultPath = "MapFiles/";
        private const string LevelName = "Level";

        private BuildRules _rules;

        public MapReader()
        {
            _rules = new BuildRules();
        }

        public MapData GetMapData(int levelIndex)
        {
            string levelPath = $"{LevelMapDefaultPath}{LevelName}{levelIndex}";
            return _rules.Convert(ReadText(levelPath));
        }
    }
}