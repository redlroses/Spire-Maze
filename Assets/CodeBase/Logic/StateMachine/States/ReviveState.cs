﻿using CodeBase.Infrastructure.States;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Player;

namespace CodeBase.Logic.StateMachine.States
{
    public class ReviveState : IState
    {
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly HeroAnimator _heroAnimator;
        private readonly PlayerHealth _playerHealth;

        public ReviveState(PlayerStateMachine playerStateMachine, HeroAnimator heroAnimator, PlayerHealth playerHealth)
        {
            _playerStateMachine = playerStateMachine;
            _heroAnimator = heroAnimator;
            _playerHealth = playerHealth;
        }

        public void Enter()
        {
            _heroAnimator.PlayRevive();
            _playerHealth.Heal(_playerHealth.MaxPoints);
            _heroAnimator.StateExited += OnExitReviveState;
        }

        private void OnExitReviveState(AnimatorState state)
        {
            if (state == AnimatorState.Revive)
            {
                _playerStateMachine.Enter<PlayerIdleState>();
            }
        }

        public void Exit() { }
    }
}