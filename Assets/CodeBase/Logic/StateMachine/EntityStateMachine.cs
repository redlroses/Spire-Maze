using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.States;

namespace CodeBase.Logic.StateMachine
{
    public abstract class EntityStateMachine
    {
        protected readonly Dictionary<Type, IExitableState> States;

        private IExitableState _activeState;

        protected EntityStateMachine()
        {
            States = new Dictionary<Type, IExitableState>();
        }

        public virtual void Cleanup()
        {
            _activeState?.Exit();
            _activeState = null;
        }

        public void Enter<TState>()
            where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload)
            where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>()
            where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>()
            where TState : class, IExitableState =>
            States[typeof(TState)] as TState;
    }
}