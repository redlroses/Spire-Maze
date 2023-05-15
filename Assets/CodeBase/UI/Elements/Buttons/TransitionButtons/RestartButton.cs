using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    class RestartButton : TransitionButton
    {
        private int _levelId;

        public void Construct(GameStateMachine stateMachine, int levelId)
        {
            StateMachine = stateMachine;
            _levelId = levelId;
        }

        protected override LoadPayload TransitionPayload() =>
            new LoadPayload(LevelNames.BuildableLevel, true,
                _levelId, true);
    }
}