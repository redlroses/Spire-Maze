using System;
using CodeBase.Services.Pause;

namespace CodeBase.Services.Watch
{
    public interface IWatchService : IService
    {
        event Action<float> TimeChanged;
        void Start();
        void ClearUp();
        void RegisterPauseWatcher(IPauseReactive pauseService);
        void LoadProgress();
        void UpdateProgress();
    }
}