using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    [RequireComponent(typeof(Button))]
    public abstract class TransitionButton : ButtonObserver
    {
        private GameStateMachine _stateMachine;

        public void Construct(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        protected override void OnCall() =>
            _stateMachine.Enter<LoadLevelState, LoadPayload>(CreateTransitionPayload());

        protected abstract LoadPayload CreateTransitionPayload();
    }
}