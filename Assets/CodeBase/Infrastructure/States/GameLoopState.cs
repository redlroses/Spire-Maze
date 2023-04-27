using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class GameLoopState : IState
  {
    private IPersistentProgressService _progressService;
    private ISaveLoadService _saveLoadService;
    private GameStateMachine _stateMachine;

    public GameLoopState(GameStateMachine stateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
    {
      _stateMachine = stateMachine;
      _saveLoadService = saveLoadService;
      _progressService = progressService;
    }

    public void Exit()
    {
    }

    public void Enter()
    {
      _progressService.Progress.WorldData.SceneName = CurrentScene();
      _saveLoadService.SaveProgress();
    }

    private string CurrentScene() =>
      SceneManager.GetActiveScene().name;
  }
}