using System;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    public class RestartButton : TransitionButton
    {
        [FormerlySerializedAs("_isClearLoad")] [SerializeField]
        private bool _isResetProgressAfterRestart;

        private int _levelId;

        public void Construct(GameStateMachine stateMachine, int levelId)
        {
            StateMachine = stateMachine;
            _levelId = levelId;
        }

        protected override LoadPayload CreateTransitionPayload()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            return sceneName.Equals(LevelNames.BuildableLevel)
                ? new LoadPayload(LevelNames.BuildableLevel, true, _levelId, _isResetProgressAfterRestart, _isResetProgressAfterRestart)
                : new LoadPayload(sceneName, false, GetLevelId(sceneName), true);
        }

        private int GetLevelId(string bySceneName)
        {
            return bySceneName switch
            {
                LevelNames.Lobby => LevelNames.LobbyId,
                LevelNames.LearningLevel => LevelNames.LearningLevelId,
                _ => throw new ArgumentNullException(nameof(bySceneName))
            };
        }
    }
}