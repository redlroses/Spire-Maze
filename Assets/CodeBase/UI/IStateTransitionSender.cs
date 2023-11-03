using System;

namespace CodeBase.UI
{
    public interface IStateTransitionSender
    {
        event Action<int> StateChanged;
    }
}