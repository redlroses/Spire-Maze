using CodeBase.Infrastructure.States;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Movement;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Logic.StateMachine.States
{
    public class JumpState : IPayloadedState<MoveDirection>
    {
        private readonly EntityStateMachine _entityStateMachine;
        private readonly HeroAnimator _heroAnimator;
        private readonly IInputService _inputService;
        private readonly HeroMover _mover;

        private MoveDirection _lastDirection;

        public JumpState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator,
            IInputService inputService, HeroMover mover)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _inputService = inputService;
            _mover = mover;
        }

        public void Enter(MoveDirection isLoss)
        {
            _mover.Move(isLoss);
            _lastDirection = isLoss;
            _heroAnimator.StateExited += OnStateExited;
            _inputService.HorizontalMove += OnHorizontalMove;
        }

        public void Exit()
        {
            _heroAnimator.StateExited -= OnStateExited;
            _inputService.HorizontalMove -= OnHorizontalMove;
        }

        private void OnStateExited(AnimatorState state)
        {
            if (state != AnimatorState.Jump)
                return;

            if (_inputService.MovementPhase != InputActionPhase.Waiting)
            {
                _entityStateMachine.Enter<PlayerMoveState, MoveDirection>(_lastDirection);
            }
            else
            {
                _entityStateMachine.Enter<PlayerIdleState>();
            }
        }

        private void OnHorizontalMove(MoveDirection direction)
        {
            _mover.Move(direction);

            if (direction != MoveDirection.Stop)
            {
                _lastDirection = direction;
            }
        }
    }
}