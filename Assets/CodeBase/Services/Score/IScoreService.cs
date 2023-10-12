namespace CodeBase.Services.Score
{
    public interface IScoreService : IService
    {
        void Calculate();
        void LoadProgress();
        void UpdateProgress();
    }
}