namespace CodeBase.Services.Score
{
    public interface IScoreService : IService
    {
        int CurrentScore { get; }
        int CalculateScore();
        void LoadProgress();
        void UpdateProgress();
    }
}