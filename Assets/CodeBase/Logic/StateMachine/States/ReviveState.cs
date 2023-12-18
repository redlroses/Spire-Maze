﻿using CodeBase.Infrastructure.States;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Hero;

namespace CodeBase.Logic.StateMachine.States
{
    public class ReviveState : IState
    {
        private readonly HeroAnimator _heroAnimator;
        private readonly HeroHealth _heroHealth;
        private readonly PlayerStateMachine _playerStateMachine;

        public ReviveState(PlayerStateMachine playerStateMachine, HeroAnimator heroAnimator, HeroHealth heroHealth)
        {
            _playerStateMachine = playerStateMachine;
            _heroAnimator = heroAnimator;
            _heroHealth = heroHealth;
        }

        public void Enter()
        {
            _heroAnimator.PlayRevive();
            _heroHealth.Heal(_heroHealth.MaxPoints);
            _heroAnimator.StateExited += OnExitReviveState;
        }

        public void Exit()
        {
        }

        private void OnExitReviveState(AnimatorState state)
        {
            if (state == AnimatorState.Revive)
            {
                _playerStateMachine.Enter<IdleState>();
            }
        }
    }
}