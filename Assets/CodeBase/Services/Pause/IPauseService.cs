namespace CodeBase.Services.Pause
{
    public interface IPauseService : IService
    {
        bool IsPause { get; }
        void SetPause(bool isPause);
        void Register(IPauseWatcher pauseWatcher);
        void UnregisterAll(IPauseWatcher pauseWatcher);
        void Cleanup();
    }
}