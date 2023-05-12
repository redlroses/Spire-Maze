using CodeBase.Infrastructure.States;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine.States
{
    public sealed class PlayerIdleState : IState
    {
        private readonly EntityStateMachine _entityStateMachine;
        private readonly HeroAnimator _heroAnimator;
        private readonly IPlayerInputService _playerInputService;

        public PlayerIdleState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator, IPlayerInputService playerInputService)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _playerInputService = playerInputService;
        }

        public void Enter()
        {
            _heroAnimator.PlayIdle();
            _playerInputService.HorizontalMove += OnHorizontalMove;
            _playerInputService.Jump += OnJump;
            _playerInputService.Dodge += OnDodge;
        }

        public void Exit()
        {
            _playerInputService.HorizontalMove -= OnHorizontalMove;
            _playerInputService.Jump -= OnJump;
            _playerInputService.Dodge -= OnDodge;
        }

        private void OnDodge(MoveDirection direction)
        {
            // _entityStateMachine.Enter<DodgeState>();
        }

        private void OnJump()
        {
            _entityStateMachine.Enter<JumpState>();
        }

        private void OnHorizontalMove(MoveDirection direction)
        {
            // _entityStateMachine.Enter<PlayerMoveState>();
        }
    }
}