namespace CodeBase.Services.Score
{
    public interface IScoreService : IService
    {
        void Calculate(bool isLose);
        void LoadProgress();
        void UpdateProgress();
    }
}