namespace CodeBase.Infrastructure
{
    public readonly struct LoadPayload
    {
        public readonly string SceneName;
        public readonly int LevelId;
        public readonly bool IsClearLoad;
        public readonly bool IsBuildable;

        public LoadPayload(string sceneName, bool isBuildable, int levelId = -2, bool isClearLoad = false)
        {
            SceneName = sceneName;
            LevelId = levelId;
            IsClearLoad = isClearLoad;
            IsBuildable = isBuildable;
        }
    }
}