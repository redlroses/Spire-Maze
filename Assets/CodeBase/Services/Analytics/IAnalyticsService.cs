namespace CodeBase.Services.Analytics
{
    public interface IAnalyticsService : IService
    {
        void TrackLevelComplete(int levelId, int stars, int playTime, int artifacts, int score);

        void TrackLevelLose(int levelId, int stars, int playTime, int artifacts, int score);

        void TrackLevelStart(int levelId);
    }
}