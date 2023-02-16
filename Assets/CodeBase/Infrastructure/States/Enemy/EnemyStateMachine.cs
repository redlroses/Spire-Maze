using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.States.Enemy
{
    public class EnemyStateMachine
    {
        private Dictionary<Type, IEnemyState> _states;
        private IEnemyState _activeState;

        public void Enter<TEnemyState>() where TEnemyState : IEnemyState
        {
            IState state = ChangeState<TEnemyState>();
            state.Enter();
        }

        public void Update<TEnemyState>() where TEnemyState : IEnemyState
        {
            _activeState.Update();
        }

        private TEnemyState ChangeState<TEnemyState>() where TEnemyState : IEnemyState
        {
            _activeState?.Exit();

            TEnemyState state = GetState<TEnemyState>();
            _activeState = state;

            return state;
        }

        private TEnemyState GetState<TEnemyState>() where TEnemyState : IEnemyState =>
            (TEnemyState)_states[typeof(TEnemyState)];
    }
}