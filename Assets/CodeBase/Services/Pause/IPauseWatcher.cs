namespace CodeBase.Services.Pause
{
    public interface IPauseWatcher
    {
        void Register(IPauseReactive pauseReactive);
    }
}