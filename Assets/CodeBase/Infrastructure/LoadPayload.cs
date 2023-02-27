namespace CodeBase.Infrastructure
{
    public struct LoadPayload
    {
        public string SceneName;
        public string LevelKey;
        public bool IsBuildable;

        public LoadPayload(string sceneName, bool isBuildable, string levelKey = null)
        {
            SceneName = sceneName;
            LevelKey = levelKey;
            IsBuildable = isBuildable;
        }
    }
}