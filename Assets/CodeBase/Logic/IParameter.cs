using System;

namespace CodeBase.Logic
{
    public interface IParameter
    {
        event Action Changed;
        public int CurrentPoints { get; }
        public int MaxPoints { get; }
    }
}