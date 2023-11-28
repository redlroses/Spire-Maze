namespace CodeBase.Services.Score
{
    public interface IScoreService : IService
    {
        int Calculate(bool isLose);
    }
}