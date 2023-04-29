using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class GameLoopState : IState
  {
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadService;
    private readonly GameStateMachine _stateMachine;
    private readonly IPlayerInputService _inputService;

    public GameLoopState(GameStateMachine stateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService, IPlayerInputService inputService)
    {
      _inputService = inputService;
      _stateMachine = stateMachine;
      _saveLoadService = saveLoadService;
      _progressService = progressService;
    }

    public void Exit()
    {
      _inputService.ClearUp();
    }

    public void Enter()
    {
      _progressService.Progress.WorldData.SceneName = CurrentScene();
      _saveLoadService.SaveProgress();
      _inputService.Subscribe();
    }

    private string CurrentScene() =>
      SceneManager.GetActiveScene().name;
  }
}