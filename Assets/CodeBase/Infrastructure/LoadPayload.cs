namespace CodeBase.Infrastructure
{
    public readonly struct LoadPayload
    {
        public readonly string SceneName;
        public readonly string LevelKey;
        public readonly bool IsClearLoad;
        public readonly bool IsBuildable;

        public LoadPayload(string sceneName, bool isBuildable, string levelKey = null, bool isClearLoad = false)
        {
            SceneName = sceneName;
            LevelKey = levelKey;
            IsClearLoad = isClearLoad;
            IsBuildable = isBuildable;
        }
    }
}