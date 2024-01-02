namespace CodeBase.Infrastructure
{
    public readonly struct LoadPayload
    {
        public readonly string SceneName;
        public readonly int LevelId;
        public readonly bool IsClearLoad;
        public readonly bool IsSaveAfterLoad;
        public readonly bool IsShowAd;

#if CRAZY_GAMES
        public LoadPayload(string sceneName, int levelId = -2, bool isClearLoad = false, bool isSaveAfterLoad = false, bool isShowAd = false)
#endif
#if YANDEX_GAMES
        public LoadPayload(string sceneName, int levelId = -2, bool isClearLoad = false, bool isSaveAfterLoad = false, bool isShowAd = true)
#endif
        {
            SceneName = sceneName;
            LevelId = levelId;
            IsClearLoad = isClearLoad;
            IsSaveAfterLoad = isSaveAfterLoad;
            IsShowAd = isShowAd;
        }
    }
}