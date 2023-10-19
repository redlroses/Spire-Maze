using System;
using CodeBase.Logic.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Services.Input
{
    public interface IPlayerInputService : IService
    {
        event Action<MoveDirection> HorizontalMove;
        event Action Jump;
        event Action<MoveDirection> Dodge;
        event Action<Vector2> OverviewMove;
        event Action Action;
        InputActionPhase MovementPhase { get; }
        MoveDirection HorizontalDirection { get; }
        void Subscribe();
        void Cleanup();
        void EnableMovementMap();
        void EnableOverviewMap();
        void DisableMovementMap();
        void DisableOverviewMap();
    }
}