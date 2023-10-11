namespace CodeBase.DelayRoutines
{
    public interface IRoutine
    {
        bool IsActive { get; }
        void AddNext(IRoutine routine);
        void Play();
        void Stop();
    }
}