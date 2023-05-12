using System;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Logic.StateMachine.States
{
    public class DodgeState : IPayloadedState<MoveDirection>
    {
        private readonly EntityStateMachine _entityStateMachine;
        private readonly HeroAnimator _heroAnimator;
        private readonly IPlayerInputService _playerInputService;
        private readonly InputController _inputController;

        private MoveDirection _direction;

        public DodgeState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator,
            IPlayerInputService playerInputService)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _playerInputService = playerInputService;
            _inputController = new InputController();
        }

        public void Enter(MoveDirection direction)
        {
            _heroAnimator.PlayDodge();
            _heroAnimator.StateExited += OnStateExited;
        }

        private void OnStateExited(AnimatorState state)
        {
            if (state != AnimatorState.Dodge)
                return;

            if (_inputController.Player.Movement.phase == InputActionPhase.Performed)
            {
                Debug.Log(_inputController.Player.Movement.phase == InputActionPhase.Performed);
                _entityStateMachine.Enter<PlayerMoveState, MoveDirection>(_direction);
            }
            else
            {
                _entityStateMachine.Enter<PlayerIdleState>();
            }
        }

        public void Exit()
        {
            _heroAnimator.StateExited -= OnStateExited;
        }
    }
}