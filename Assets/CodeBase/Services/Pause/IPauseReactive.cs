using System;

namespace CodeBase.Services.Pause
{
    public interface IPauseReactive
    {
        bool IsPause { get; }
        event Action Pause;
        event Action Resume;
    }
}