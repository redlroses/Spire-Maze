using GameAnalyticsSDK;

namespace CodeBase.Services.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private const char DesignSeparator = ':';

        private const string LevelLabel = "Level";
        private const string StarsLabel = "Stars";
        private const string PlayTimeLabel = "PlayTime";
        private const string ScoreLabel = "Score";
        private const string ArtifactLabel = "Artifact";

        public void TrackLevelComplete(int levelId, int stars, int playTime, int artifacts, int score)
        {
#if !UNITY_EDITOR
            TrackLevelFinish(levelId, stars, playTime, artifacts, score, GAProgressionStatus.Complete);
#endif
        }

        public void TrackLevelLose(int levelId, int stars, int playTime, int artifacts, int score)
        {
#if !UNITY_EDITOR
            TrackLevelFinish(levelId, stars, playTime, artifacts, score, GAProgressionStatus.Fail);
#endif
        }

        public void TrackLevelStart(int levelId)
        {
#if !UNITY_EDITOR
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, GetLevelName(levelId));
#endif
        }

        private void TrackLevelFinish(
            int levelId,
            int stars,
            int playTime,
            int artifacts,
            int score,
            GAProgressionStatus status)
        {
            SendProgressEvent(levelId, score, status);
            SendDesignEvent(levelId, StarsLabel, stars, status);
            SendDesignEvent(levelId, PlayTimeLabel, playTime, status);
            SendDesignEvent(levelId, ScoreLabel, score, status);
            SendDesignEvent(levelId, ArtifactLabel, artifacts, status);
        }

        private void SendDesignEvent(int levelId, string designLabel, int value, GAProgressionStatus status)
        {
            string eventName = string.Join(DesignSeparator, GetLevelName(levelId), status, designLabel);
            GameAnalytics.NewDesignEvent(eventName, value);
        }

        private void SendProgressEvent(int levelId, int score, GAProgressionStatus status) =>
            GameAnalytics.NewProgressionEvent(status, GetLevelName(levelId), score);

        private string GetLevelName(int levelId) =>
            $"{LevelLabel}_{levelId}";
    }
}