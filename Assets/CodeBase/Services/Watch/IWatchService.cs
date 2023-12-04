using System;

namespace CodeBase.Services.Watch
{
    public interface IWatchService : IService
    {
        event Action<int> SecondTicked;
        float ElapsedTime { get; }
        int ElapsedSeconds { get; }
        void Start();
        void Stop();
        void Cleanup();
        void LoadProgress();
        void UpdateProgress();
    }
}