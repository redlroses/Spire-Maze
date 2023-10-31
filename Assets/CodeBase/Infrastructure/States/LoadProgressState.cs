using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Sound;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string PlayerKey = "Player";

        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadProgress;
        private readonly IStaticDataService _staticDataService;
        private readonly ISoundService _soundService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService,
            ISaveLoadService saveLoadProgress, IStaticDataService staticDataService, ISoundService soundService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadProgress = saveLoadProgress;
            _staticDataService = staticDataService;
            _soundService = soundService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(
                _progressService.Progress.WorldData.SceneName,
                _progressService.Progress.WorldData.SceneName == LevelNames.BuildableLevel,
                _progressService.Progress.WorldData.LevelState.LevelId));

            _soundService.Load();
        }

        public void Exit() { }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress =
                _saveLoadProgress.LoadProgress()
                ?? NewProgress();

            _progressService.TemporalProgress = new TemporalProgress();
        }

        private PlayerProgress NewProgress()
        {
            PlayerProgress progress = new PlayerProgress(initialLevel: LevelNames.Lobby)
            {
                WorldData =
                {
                    LevelPositions = new LevelPositions(
                        _staticDataService.ForLevel(LevelNames.LobbyId)
                            .HeroInitialPosition.AsVectorData(),
                        _staticDataService.ForLevel(LevelNames.LobbyId)
                            .FinishPosition.AsVectorData()),
                    HeroHealthState = new HealthState(_staticDataService.HealthForEntity(PlayerKey).MaxHealth),
                    HeroStaminaState = new StaminaState(_staticDataService.StaminaForEntity(PlayerKey).MaxStamina)
                },
            };

            progress.GlobalData.SoundVolume.Reset();

            return progress;
        }
    }
}