using System;

namespace CodeBase.Logic
{
    public interface IBar
    {
        event Action Changed;
        int CurrentPoints { get; }
        int MaxPoints { get; }
    }
}