using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using UnityEngine.SceneManagement;

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
            SceneManager.GetActiveScene().name.Equals(LevelNames.LearningLevel)
                ? new LoadPayload(LevelNames.Lobby, false, LevelNames.LobbyId)
                : new LoadPayload(LevelNames.BuildableLevel, true, _levelId + 1);
    }
}