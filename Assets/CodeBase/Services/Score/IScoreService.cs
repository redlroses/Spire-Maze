namespace CodeBase.Services.Score
{
    public interface IScoreService : IService
    {
        int CalculateScore();
        void LoadProgress();
        void UpdateProgress();
    }
}