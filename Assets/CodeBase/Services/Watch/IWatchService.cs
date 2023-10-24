using System;
using CodeBase.Services.Pause;

namespace CodeBase.Services.Watch
{
    public interface IWatchService : IService, IPauseWatcher
    {
        event Action<float> TimeChanged;
        float ElapsedTime { get; }
        void Start();
        void Cleanup();
        void LoadProgress();
        void UpdateProgress();
    }
}