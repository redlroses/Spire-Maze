using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.Pause;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button _unpauseButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

        private IPauseService _pauseService;
        private GameStateMachine _stateMachine;

        public void Construct(IPauseService pauseService, GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _pauseService = pauseService;
            _pauseService.Resume += OnResume;
        }

        private void OnResume()
        {
            _pauseService.Resume -= OnResume;
            Destroy(gameObject);
        }

        protected override void SubscribeUpdates()
        {
            _unpauseButton.onClick.AddListener(() => _pauseService.SetPause(false));
            _menuButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.TestLevelName, true,
                    LevelNames.FirstLevelKey)));
            _restartButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.TestLevelName, true,
                    LevelNames.FirstLevelKey, true)));
        }

        protected override void CleanUp()
        {
            _unpauseButton.onClick.RemoveAllListeners();
        }
    }
}