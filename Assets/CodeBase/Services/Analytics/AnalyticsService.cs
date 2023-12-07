using GameAnalyticsSDK;
using UnityEngine;

namespace CodeBase.Services.Analytics
{
    public class AnalyticsService
    {
        private const string LevelLabel = "Level_";
        private const string StarsLabel = "Stars";
        private const string PlayTimeLabel = "PlayTime";
        private const string ScoreLabel = "Score";

        public void TrackLevelComplete(int levelId, int stars, int playTime, int score)
        {
            SendProgressEvent(levelId, StarsLabel, stars, GAProgressionStatus.Complete);
        }

        private void SendProgressEvent(int levelId, string starsLabel, int stars, GAProgressionStatus status)
        {
            string level = $"{LevelLabel}{levelId}";
            // GameAnalytics.NewProgressionEvent(status, level,  )
        }
    }
}