using CodeBase.Infrastructure.States;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Movement;
using CodeBase.Services.Input;
using UnityEngine.InputSystem;

namespace CodeBase.Logic.StateMachine.States
{
    public class DodgeState : IPayloadedState<MoveDirection>
    {
        private readonly EntityStateMachine _entityStateMachine;
        private readonly HeroAnimator _heroAnimator;
        private readonly IInputService _inputService;
        private readonly IImmune _immune;
        private readonly InputController _inputController;

        private MoveDirection _lastDirection;

        public DodgeState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator,
            IInputService inputService, IImmune immune)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _inputService = inputService;
            _immune = immune;
        }

        public void Enter(MoveDirection isLoss)
        {
            _heroAnimator.PlayDodge();
            _lastDirection = isLoss;
            _immune.ActivateImmunity();
            _inputService.HorizontalMove += OnHorizontalMove;
            _heroAnimator.StateExited += OnStateExited;
        }

        public void Exit()
        {
            _immune.DeactivateImmunity();
            _heroAnimator.StateExited -= OnStateExited;
            _inputService.HorizontalMove -= OnHorizontalMove;
        }

        private void OnHorizontalMove(MoveDirection direction)
        {
            if (direction != MoveDirection.Stop)
            {
                _lastDirection = direction;
            }
        }

        private void OnStateExited(AnimatorState state)
        {
            if (state != AnimatorState.Dodge)
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
    }
}