using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Logic.MonoStateMachine
{
    public abstract class State : MonoBehaviour, IMonoState
    {
        [SerializeField] private List<Transition> _toTransitions;

        private readonly List<ITransition> _transitions = new List<ITransition>();

        private void Awake()
        {
            foreach (Transition transition in _toTransitions)
            {
                _transitions.Add(transition);

                OnAwake();
            }
        }

        public void EnterBehavior()
        {
            enabled = true;
            EnableTransitions();
        }

        public void ExitBehavior()
        {
            enabled = false;
            DisableTransitions();
        }

        protected virtual void OnAwake() { }

        private void EnableTransitions()
        {
            foreach (ITransition transition in _transitions)
            {
                transition.Enable();
            }
        }

        private void DisableTransitions()
        {
            foreach (ITransition transition in _transitions)
            {
                transition.Disable();
            }
        }
    }
}