using System;
using CodeBase.Logic.Movement;

namespace CodeBase.Services.Input
{
    public interface IPlayerInputService : IService
    {
        event Action<MoveDirection> HorizontalMove;
        event Action Jump;
        event Action<MoveDirection> Dodge;
        void Subscribe();
        void ClearUp();
    }
}