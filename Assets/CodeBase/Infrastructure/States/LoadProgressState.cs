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
        private readonly ISoundService _soundService;
        private readonly IStaticDataService _staticDataService;

        public LoadProgressState(
            GameStateMachine gameStateMachine,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadProgress,
            IStaticDataService staticDataService,
            ISoundService soundService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadProgress = saveLoadProgress;
            _staticDataService = staticDataService;
            _soundService = soundService;
        }

        public async void Enter()
        {
            await InitProgress();

            _gameStateMachine.Enter<LoadLevelState, LoadPayload>(
                new LoadPayload(
                    _progressService.Progress.WorldData.SceneName,
                    _progressService.Progress.WorldData.LevelState.LevelId));

            _soundService.Load();
        }

        public void Exit()
        {
        }

        private async UniTask InitProgress()
        {
            PlayerProgress loaded = await _saveLoadProgress.LoadProgress();
            _progressService.Progress = loaded ?? NewProgress();
            _progressService.TemporalProgress = new TemporalProgress();
        }

        private PlayerProgress NewProgress()
        {
            PlayerProgress progress =
                new PlayerProgress(LevelNames.LearningLevel, LevelNames.LearningLevelId)
                {
                    WorldData =
                    {
                        LevelPositions = new LevelPositions(
                            _staticDataService.GetLevel(LevelNames.LearningLevelId)
                                .HeroInitialPosition.AsVectorData(),
                            _staticDataService.GetLevel(LevelNames.LearningLevelId)
                                .FinishPosition.AsVectorData()),
                        HeroHealthState = new HealthState(_staticDataService.GetHealthEntity(PlayerKey).MaxHealth),
                        HeroStaminaState = new StaminaState(_staticDataService.GetStaminaEntity(PlayerKey).MaxStamina),
                    },
                };

            progress.GlobalData.SoundVolume.Reset();

            return progress;
        }
    }
}