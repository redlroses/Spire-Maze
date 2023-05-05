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
    private readonly IPlayerInputService _inputService;
    private readonly IWatchService _watchService;

    public GameLoopState(GameStateMachine stateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService,
      IPlayerInputService inputService, IWatchService watchService)
    {
      _inputService = inputService;
      _stateMachine = stateMachine;
      _saveLoadService = saveLoadService;
      _progressService = progressService;
      _watchService = watchService;
    }

    public void Exit()
    {
      _saveLoadService.SaveProgress();
      _inputService.ClearUp();
      _watchService.Cleanup();
    }

    public void Enter()
    {
      _progressService.Progress.WorldData.SceneName = CurrentScene();
      _saveLoadService.SaveProgress();
      _inputService.Subscribe();
      _watchService.Start();
    }

    private string CurrentScene() =>
      SceneManager.GetActiveScene().name;
  }
}