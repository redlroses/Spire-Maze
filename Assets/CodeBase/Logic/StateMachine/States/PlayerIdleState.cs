﻿using CodeBase.Infrastructure.States;
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
        private readonly HeroMover _mover;
        private readonly Jumper _jumper;
        private readonly Dodge _dodge;

        public PlayerIdleState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator,
            IPlayerInputService playerInputService, HeroMover mover, Jumper jumper, Dodge dodge)
        {
            _entityStateMachine = entityStateMachine;
            _heroAnimator = heroAnimator;
            _playerInputService = playerInputService;
            _mover = mover;
            _jumper = jumper;
            _dodge = dodge;
        }

        public void Enter()
        {
            _mover.Move(MoveDirection.Stop);
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
            if (_dodge.CanDodge)
            {
                _entityStateMachine.Enter<DodgeState, MoveDirection>(direction);
            }
        }

        private void OnJump()
        {
            if (_jumper.CanJump)
            {
                _entityStateMachine.Enter<JumpState, MoveDirection>(MoveDirection.Stop);
            }
        }

        private void OnHorizontalMove(MoveDirection direction)
        {
            _entityStateMachine.Enter<PlayerMoveState, MoveDirection>(direction);
        }
    }
}