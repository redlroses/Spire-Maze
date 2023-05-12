using CodeBase.Infrastructure.States;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine.States
{
    public sealed class PlayerMoveState : IPayloadedState<MoveDirection>
    {
        private readonly EntityStateMachine _entityStateMachine;
        private readonly HeroAnimator _heroAnimator;
        private readonly IPlayerInputService _playerInputService;
        private readonly HeroMover _mover;

        public PlayerMoveState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator,
            IPlayerInputService playerInputService, HeroMover mover)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _playerInputService = playerInputService;
            _mover = mover;
        }

        public void Enter(MoveDirection direction)
        {
            _heroAnimator.PlayRun(true);
            _mover.Move(direction);
            _playerInputService.HorizontalMove += OnHorizontalMove;
            _playerInputService.Jump += OnJump;
            _playerInputService.Dodge += OnDodge;
        }

        private void OnDodge(MoveDirection direction)
        {
            _entityStateMachine.Enter<DodgeState, MoveDirection>(direction);
        }

        private void OnJump()
        {
            _entityStateMachine.Enter<JumpState>();
        }

        private void OnHorizontalMove(MoveDirection direction)
        {
            _mover.Move(direction);
        }

        public void Exit()
        {
            _heroAnimator.PlayRun(false);
            _playerInputService.HorizontalMove -= OnHorizontalMove;
        }
    }
}