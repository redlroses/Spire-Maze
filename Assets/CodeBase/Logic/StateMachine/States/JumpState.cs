using CodeBase.Infrastructure.States;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Logic.StateMachine.States
{
    public class JumpState : IPayloadedState<MoveDirection>
    {
        private readonly EntityStateMachine _entityStateMachine;
        private readonly HeroAnimator _heroAnimator;
        private readonly IPlayerInputService _playerInputService;
        private readonly Jumper _jumper;
        private readonly HeroMover _mover;

        private MoveDirection _lastDirection;

        public JumpState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator,
            IPlayerInputService playerInputService, Jumper jumper, HeroMover mover)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _playerInputService = playerInputService;
            _jumper = jumper;
            _mover = mover;
        }

        public void Enter(MoveDirection isLose)
        {
            _jumper.Jump();
            _mover.Move(isLose);
            _lastDirection = isLose;
            _heroAnimator.StateExited += OnStateExited;
            _playerInputService.HorizontalMove += OnHorizontalMove;
        }

        public void Exit()
        {
            _heroAnimator.StateExited -= OnStateExited;
            _playerInputService.HorizontalMove -= OnHorizontalMove;
        }

        private void OnStateExited(AnimatorState state)
        {
            if (state != AnimatorState.Jump)
                return;

            if (_playerInputService.MovementPhase != InputActionPhase.Waiting)
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