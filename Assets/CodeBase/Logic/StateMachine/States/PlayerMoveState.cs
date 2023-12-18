using CodeBase.Infrastructure.States;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Movement;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine.States
{
    public sealed class MoveState : IPayloadedState<MoveDirection>
    {
        private readonly EntityStateMachine _entityStateMachine;
        private readonly IInputService _inputService;
        private readonly HeroMover _mover;
        private readonly Jumper _jumper;
        private readonly Dodge _dodge;

        private MoveDirection _lastDirection;

        public MoveState(EntityStateMachine entityStateMachine,
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

        public void Enter(MoveDirection isLoss)
        {
            _mover.Move(isLoss);
            _lastDirection = isLoss;
            _inputService.HorizontalMove += OnHorizontalMove;
            _inputService.Jump += OnJump;
            _inputService.Dodge += OnDodge;
        }

        public void Exit()
        {
            _inputService.HorizontalMove -= OnHorizontalMove;
            _inputService.Jump -= OnJump;
            _inputService.Dodge -= OnDodge;
        }

        private void OnDodge(MoveDirection direction)
        {
            if (_dodge.TryDodge(direction))
                _entityStateMachine.Enter<DodgeState, MoveDirection>(direction);
        }

        private void OnJump()
        {
            if (_jumper.TryJump())
                _entityStateMachine.Enter<JumpState, MoveDirection>(_lastDirection);
        }

        private void OnHorizontalMove(MoveDirection direction)
        {
            _mover.Move(direction);
            _lastDirection = direction;

            if (direction != MoveDirection.Stop)
                return;

            _entityStateMachine.Enter<IdleState>();
        }
    }
}