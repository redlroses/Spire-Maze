using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadProgress = saveLoadProgress;
    }

    public void Enter()
    {
      LoadProgressOrInitNew();
    //  _gameStateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.TestLevelName, true, LevelNames.FirstLevelKey));
      _gameStateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(LevelNames.TestLevelTwoName, true, LevelNames.FirstLevelKey2));

        }

        public void Exit()
    {
    }

    private void LoadProgressOrInitNew()
    {
      _progressService.Progress =
        _saveLoadProgress.LoadProgress()
        ?? NewProgress();
    }

    private PlayerProgress NewProgress()
    {
      var progress = new PlayerProgress(initialLevel: LevelNames.TestLevelName)
      {
        HeroHealthState = {MaxHP = 50}
      };

      progress.HeroHealthState.ResetHP();

      return progress;
    }
  }
}