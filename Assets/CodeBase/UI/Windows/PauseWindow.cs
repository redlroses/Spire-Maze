using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : WindowBase, IPauseWatcher
    {
        [SerializeField] private Button _unpauseButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

        private IPauseService _pauseService;
        private GameStateMachine _stateMachine;
        private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService, IPauseService pauseService, GameStateMachine stateMachine)
        {
            _pauseService = pauseService;
            _progressService = progressService;
            _stateMachine = stateMachine;
        }

        public void Pause()
        {
        }

        public void Resume()
        {
            _pauseService.UnregisterAll(this);
            Destroy(gameObject);
        }

        protected override void SubscribeUpdates()
        {
            _unpauseButton.onClick.AddListener(() => _pauseService.SetPause(false));
            _menuButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.Lobby, false,
                    LevelNames.LobbyId)));
            _restartButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.BuildableLevel, true,
                    _progressService.Progress.WorldData.LevelState.LevelId, true)));
        }

        protected override void Cleanup()
        {
            _unpauseButton.onClick.RemoveAllListeners();
        }
    }
}