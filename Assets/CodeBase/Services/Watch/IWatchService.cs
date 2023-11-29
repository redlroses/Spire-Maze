using System;
using CodeBase.Services.Pause;

namespace CodeBase.Services.Watch
{
    public interface IWatchService : IService, IPauseWatcher
    {
        event Action<int> SecondTicked;
        float ElapsedTime { get; }
        int ElapsedSeconds { get; }
        void Start();
        void Cleanup();
        void LoadProgress();
        void UpdateProgress();
    }
}