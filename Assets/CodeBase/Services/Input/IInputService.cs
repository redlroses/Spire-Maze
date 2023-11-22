using System;
using CodeBase.Logic.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        event Action<MoveDirection> HorizontalMove;
        event Action Jump;
        event Action<MoveDirection> Dodge;
        event Action<Vector2> OverviewMove;
        event Action Deactivated;
        InputActionPhase MovementPhase { get; }
        void Subscribe();
        void Cleanup();
        void EnableMovementMap();
        void EnableOverviewMap();
        void DisableMovementMap();
        void DisableOverviewMap();
    }
}