namespace CodeBase.Services.Pause
{
    public interface IPauseWatcher
    {
        void RegisterPauseWatcher(IPauseReactive pauseReactive);
    }
}