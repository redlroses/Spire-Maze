namespace CodeBase.Infrastructure
{
    public readonly struct LoadPayload
    {
        public readonly string SceneName;
        public readonly string LevelKey;
        public readonly bool IsBuildable;

        public LoadPayload(string sceneName, bool isBuildable, string levelKey = null)
        {
            SceneName = sceneName;
            LevelKey = levelKey;
            IsBuildable = isBuildable;
        }
    }
}