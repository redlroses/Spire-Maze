using System;
using CodeBase.Logic.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        event Action<MoveDirection> HorizontalMoving;

        event Action Jumped;

        event Action<MoveDirection> Dodged;

        event Action<Vector2> OverviewMoving;

        event Action MoveStopped;

        InputActionPhase MovementPhase { get; }

        void Subscribe();

        void Cleanup();

        void EnableMovementMap();

        void EnableOverviewMap();

        void DisableMovementMap();

        void DisableOverviewMap();
    }
}