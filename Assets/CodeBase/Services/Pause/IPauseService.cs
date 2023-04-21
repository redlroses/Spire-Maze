namespace CodeBase.Services.Pause
{
    public interface IPauseService : IPauseReactive, IService
    {
        void SetPause(bool isPause);
    }
}