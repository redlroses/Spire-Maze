using CodeBase.Tools;

namespace CodeBase.Services.Pause
{
    public interface IPauseService : IService
    {
        bool IsPause { get; }
        void Register(IPauseWatcher pauseWatcher);
        void Unregister(IPauseWatcher pauseWatcher);
        void Cleanup();
        void EnablePause(Locker locker = null);
        void DisablePause(Locker locker = null);
    }
}