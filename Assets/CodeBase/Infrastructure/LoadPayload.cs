namespace CodeBase.Infrastructure
{
    public readonly struct LoadPayload
    {
        public readonly string SceneName;
        public readonly int LevelId;
        public readonly bool IsClearLoad;
        public readonly bool IsBuildable;
        public readonly bool IsSaveAfterLoad;

        public LoadPayload(string sceneName, bool isBuildable, int levelId = -2, bool isClearLoad = false, bool isSaveAfterLoad = false)
        {
            SceneName = sceneName;
            LevelId = levelId;
            IsClearLoad = isClearLoad;
            IsBuildable = isBuildable;
            IsSaveAfterLoad = isSaveAfterLoad;
        }
    }
}