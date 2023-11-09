using System;

namespace CodeBase.Logic
{
    public interface IPoints
    {
        event Action Changed;
        public int CurrentPoints { get; }
        public int MaxPoints { get; }
    }
}