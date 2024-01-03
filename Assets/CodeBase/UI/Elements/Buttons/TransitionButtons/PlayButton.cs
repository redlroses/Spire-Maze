using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    public class PlayButton : TransitionButton
    {
        private IPersistentProgressService _progressService;

        public void Construct(GameStateMachine stateMachine, IPersistentProgressService progressService)
        {
            Construct(stateMachine);
            _progressService = progressService;
            Subscribe();
        }

        protected override LoadPayload CreateTransitionPayload() =>
            new LoadPayload(
                _progressService.Progress.WorldData.SceneName,
                _progressService.Progress.WorldData.LevelState.LevelId);
    }
}