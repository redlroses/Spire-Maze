using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    [RequireComponent(typeof(Button))]
    public abstract class TransitionButton : ButtonObserver
    {
        protected GameStateMachine StateMachine;

        public void Construct(GameStateMachine stateMachine) =>
            StateMachine = stateMachine;

        protected override void Call()
        {
            StateMachine.Enter<LoadLevelState, LoadPayload>(TransitionPayload());
        }

        protected abstract LoadPayload TransitionPayload();
    }
}