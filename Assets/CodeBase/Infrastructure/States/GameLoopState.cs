using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Watch;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class GameLoopState : IState
  {
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadService;
    private readonly GameStateMachine _stateMachine;
    private readonly IInputService _inputService;
    private readonly IWatchService _watchService;

    public GameLoopState(GameStateMachine stateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService,
      IInputService inputService, IWatchService watchService)
    {
      _inputService = inputService;
      _stateMachine = stateMachine;
      _saveLoadService = saveLoadService;
      _progressService = progressService;
      _watchService = watchService;
    }

    public void Exit()
    {
      _inputService.Cleanup();
      _watchService.Cleanup();
    }

    public void Enter()
    {
      _progressService.Progress.WorldData.SceneName = CurrentScene();
      _saveLoadService.SaveProgress();
      _inputService.Subscribe();
      _inputService.EnableMovementMap();
      _watchService.Start();
    }

    private string CurrentScene() =>
      SceneManager.GetActiveScene().name;
  }
}