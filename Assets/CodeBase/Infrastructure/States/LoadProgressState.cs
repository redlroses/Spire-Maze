using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string PlayerKey = "Player";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadProgress;
        private readonly IStaticDataService _staticDataService;

        private bool _isNewProgress;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService,
            ISaveLoadService saveLoadProgress, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadProgress = saveLoadProgress;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(
                _progressService.Progress.WorldData.LevelName,
                _progressService.Progress.WorldData.LevelName == LevelNames.BuildableLevel,
                _progressService.Progress.WorldData.LevelState.LevelKey, _isNewProgress));
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
            var progress = new PlayerProgress(initialLevel: LevelNames.Lobby)
            {
                WorldData =
                {
                    PositionOnLevel = new PositionOnLevel(LevelNames.Lobby,
                        _staticDataService.ForLevel(LevelNames.Lobby).HeroInitialPosition.AsVectorData())
                },
                HeroHealthState = {MaxHP = _staticDataService.HealthForEntity(PlayerKey).MaxHealth},
                HeroStaminaState = {MaxValue = _staticDataService.StaminaForEntity(PlayerKey).MaxStamina}
            };

            progress.HeroHealthState.ResetHP();
            progress.HeroStaminaState.ResetStamina();
            _isNewProgress = true;
            return progress;
        }
    }
}