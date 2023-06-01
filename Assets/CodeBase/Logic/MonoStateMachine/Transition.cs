using UnityEngine;

namespace CodeBase.Logic.MonoStateMachine
{
    public abstract class Transition : MonoBehaviour, ITransition
    {
        protected MonoStateMachine StateMachine;

        public void Init(MonoStateMachine stateMachine) => StateMachine = stateMachine;

        public void Enable() => enabled = true;

        public void Disable() => enabled = false;
    }
}