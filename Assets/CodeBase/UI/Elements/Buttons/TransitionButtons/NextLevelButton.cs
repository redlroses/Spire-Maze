using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    class NextLevelButton : TransitionButton
    {
        private int _levelId;

        public void Construct(GameStateMachine stateMachine, int levelId)
        {
            StateMachine = stateMachine;
            _levelId = levelId;
        }

        protected override LoadPayload TransitionPayload() =>
            new LoadPayload(LevelNames.BuildableLevel, true, _levelId + 1);
    }
}