using System;

namespace CodeBase.Logic
{
    public interface IParameter
    {
        event Action Changed;
        int CurrentPoints { get; }
        int MaxPoints { get; }
    }
}