using CodeBase.Infrastructure.States;
using CodeBase.Logic.Movement;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine.States
{
    public sealed class IdleState : IState
    {
        private readonly Dodge _dodge;
        private readonly EntityStateMachine _entityStateMachine;
        private readonly IInputService _inputService;
        private readonly Jumper _jumper;
        private readonly HeroMover _mover;

        public IdleState(
            EntityStateMachine entityStateMachine,
            IInputService inputService,
            HeroMover mover,
            Jumper jumper,
            Dodge dodge)
        {
            _entityStateMachine = entityStateMachine;
            _inputService = inputService;
            _mover = mover;
            _jumper = jumper;
            _dodge = dodge;
        }

        public void Enter()
        {
            _mover.Move(MoveDirection.Stop);
            _inputService.HorizontalMoving += OnHorizontalMove;
            _inputService.Jumped += OnJump;
            _inputService.Dodged += OnDodge;
        }

        public void Exit()
        {
            _inputService.HorizontalMoving -= OnHorizontalMove;
            _inputService.Jumped -= OnJump;
            _inputService.Dodged -= OnDodge;
        }

        private void OnDodge(MoveDirection direction)
        {
            if (_dodge.TryDodge(direction))
                _entityStateMachine.Enter<DodgeState, MoveDirection>(direction);
        }

        private void OnJump()
        {
            if (_jumper.TryJump())
                _entityStateMachine.Enter<JumpState, MoveDirection>(MoveDirection.Stop);
        }

        private void OnHorizontalMove(MoveDirection direction) =>
            _entityStateMachine.Enter<MoveState, MoveDirection>(direction);
    }
}