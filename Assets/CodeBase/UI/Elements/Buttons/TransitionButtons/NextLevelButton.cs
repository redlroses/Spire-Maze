using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.StaticData;
using UnityEngine.SceneManagement;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    public class NextLevelButton : TransitionButton
    {
        private int _levelId;
        private IStaticDataService _staticData;

        public void Construct(GameStateMachine stateMachine, IStaticDataService staticData, int levelId)
        {
            _staticData = staticData;
            StateMachine = stateMachine;
            _levelId = levelId;
        }

        protected override LoadPayload CreateTransitionPayload()
        {
            if (IsLastLevel())
                return CreateToLobbyPayload();

            return SceneManager.GetActiveScene().name.Equals(LevelNames.LearningLevel)
                ? CreateToLobbyPayload()
                : CreateToNextLevelPayload();
        }

        private LoadPayload CreateToNextLevelPayload() =>
            new LoadPayload(LevelNames.BuildableLevel, _levelId + 1, true, true);

        private static LoadPayload CreateToLobbyPayload() =>
            new LoadPayload(LevelNames.Lobby, LevelNames.LobbyId, true, true);

        private bool IsLastLevel() =>
            !_staticData.HasLevel(_levelId + 1);
    }
}