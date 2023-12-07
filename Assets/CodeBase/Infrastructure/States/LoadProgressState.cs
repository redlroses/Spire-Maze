using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Sound;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;
using Cysharp.Threading.Tasks;
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

        public async void Enter()
        {
            await LoadProgressOrInitNew();

            _gameStateMachine.Enter<LoadLevelState, LoadPayload>(new LoadPayload(
                _progressService.Progress.WorldData.SceneName,
                _progressService.Progress.WorldData.LevelState.LevelId));

            _soundService.Load();
        }

        public void Exit() { }

        private async UniTask LoadProgressOrInitNew()
        {
            _progressService.Progress =
                await _saveLoadProgress.LoadProgress()
                ?? NewProgress();

            _progressService.TemporalProgress = new TemporalProgress();
        }

        private PlayerProgress NewProgress()
        {
            PlayerProgress progress = new PlayerProgress(initialLevel: LevelNames.LearningLevel, LevelNames.LearningLevelId)
            {
                WorldData =
                {
                    LevelPositions = new LevelPositions(
                        _staticDataService.GetLevel(LevelNames.LearningLevelId)
                            .HeroInitialPosition.AsVectorData(),
                        _staticDataService.GetLevel(LevelNames.LearningLevelId)
                            .FinishPosition.AsVectorData()),
                    HeroHealthState = new HealthState(_staticDataService.GetHealthEntity(PlayerKey).MaxHealth),
                    HeroStaminaState = new StaminaState(_staticDataService.GetStaminaEntity(PlayerKey).MaxStamina)
                },
            };

            progress.GlobalData.SoundVolume.Reset();

            return progress;
        }
    }
}