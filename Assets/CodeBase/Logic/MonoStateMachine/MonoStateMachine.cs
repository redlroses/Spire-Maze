using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Logic.MonoStateMachine
{
    public abstract class MonoStateMachine : MonoBehaviour
    {
        private Dictionary<Type, IMonoState> _allBehaviors;
        private IMonoState _currentBehavior;

        private void Start()
        {
            InitStates();
            InitTransitions();
            SetDefaultState();
        }

        protected abstract void InitTransitions();

        protected abstract void SetDefaultState();

        protected abstract Dictionary<Type, IMonoState> GetStates();

        public void ChangeState<TState>() where TState : IMonoState
        {
            IMonoState behavior = _allBehaviors[typeof(TState)];
            _currentBehavior?.ExitBehavior();
            behavior.EnterBehavior();
            _currentBehavior = behavior;
        }

        private void InitStates() => _allBehaviors = GetStates();
    }
}