using System;

namespace CodeBase.Logic
{
    public interface IActivated
    {
        event Action Activated;
    }
}