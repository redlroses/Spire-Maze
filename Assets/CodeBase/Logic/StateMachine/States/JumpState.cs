using CodeBase.Infrastructure.States;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Movement;
using CodeBase.Services.Input;
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

        public JumpState(
            EntityStateMachine entityStateMachine,
            HeroAnimator heroAnimator,
            IInputService inputService,
            HeroMover mover)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _inputService = inputService;
            _mover = mover;
        }

        public void Enter(MoveDirection loadPayload)
        {
            _mover.Move(loadPayload);
            _lastDirection = loadPayload;
            _heroAnimator.StateExited += OnStateExited;
            _inputService.HorizontalMoving += OnHorizontalMove;
        }

        public void Exit()
        {
            _heroAnimator.StateExited -= OnStateExited;
            _inputService.HorizontalMoving -= OnHorizontalMove;
        }

        private void OnStateExited(AnimatorState state)
        {
            if (state != AnimatorState.Jump)
                return;

            if (_inputService.MovementPhase != InputActionPhase.Waiting)
            {
                _entityStateMachine.Enter<MoveState, MoveDirection>(_lastDirection);
            }
            else
            {
                _entityStateMachine.Enter<IdleState>();
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