using CodeBase.Infrastructure.States;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;
using UnityEngine.InputSystem;

namespace CodeBase.Logic.StateMachine.States
{
    public class DodgeState : IPayloadedState<MoveDirection>
    {
        private readonly EntityStateMachine _entityStateMachine;
        private readonly HeroAnimator _heroAnimator;
        private readonly IPlayerInputService _playerInputService;
        private readonly Dodge _dodge;
        private readonly PlayerHealth _playerHealth;
        private readonly InputController _inputController;

        private MoveDirection _lastDirection;

        public DodgeState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator,
            IPlayerInputService playerInputService, Dodge dodge, PlayerHealth playerHealth)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _playerInputService = playerInputService;
            _dodge = dodge;
            _playerHealth = playerHealth;
        }

        public void Enter(MoveDirection payload)
        {
            _heroAnimator.PlayDodge();
            _dodge.Evade(payload);
            _lastDirection = payload;
            _playerHealth.IsImmune = true;
            _playerInputService.HorizontalMove += OnHorizontalMove;
            _heroAnimator.StateExited += OnStateExited;
        }

        public void Exit()
        {
            _playerHealth.IsImmune = false;
            _heroAnimator.StateExited -= OnStateExited;
            _playerInputService.HorizontalMove -= OnHorizontalMove;
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

            if (_playerInputService.MovementPhase != InputActionPhase.Waiting)
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