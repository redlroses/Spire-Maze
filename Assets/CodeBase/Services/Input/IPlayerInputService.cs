using System;
using CodeBase.Logic.Movement;
using UnityEngine.InputSystem;

namespace CodeBase.Services.Input
{
    public interface IPlayerInputService : IService
    {
        event Action<MoveDirection> HorizontalMove;
        event Action Jump;
        event Action<MoveDirection> Dodge;
        InputActionPhase MovementPhase { get; }
        MoveDirection HorizontalDirection { get; }
        void Subscribe();
        void Cleanup();
    }
}