using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.StaticData;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    public class NextLevelButton : TransitionButton
    {
        private int _levelId;
        private IStaticDataService _staticData;

        public void Construct(GameStateMachine stateMachine, IStaticDataService staticData, int levelId)
        {
            Construct(stateMachine);
            _staticData = staticData;
            _levelId = levelId;
        }

        protected override LoadPayload CreateTransitionPayload() =>
            IsLastLevel() ? CreateToLobbyPayload() : CreateToNextLevelPayload();

        private LoadPayload CreateToNextLevelPayload() =>
            new LoadPayload(LevelNames.BuildableLevel, _levelId + 1, true, true);

        private LoadPayload CreateToLobbyPayload() =>
            new LoadPayload(LevelNames.Lobby, LevelNames.LobbyId, true, true);

        private bool IsLastLevel() =>
            !_staticData.HasLevel(_levelId + 1);
    }
}