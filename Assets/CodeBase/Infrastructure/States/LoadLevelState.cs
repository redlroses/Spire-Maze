using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification;
using CodeBase.Logic;
using CodeBase.Services.LevelBuild;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<LoadPayload>
    {
        private const string PlayerHealthKey = "Player";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly ILevelBuilder _levelBuilder;

        private LoadPayload _loadPayload;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory,
            IPersistentProgressService progressService, IStaticDataService staticDataService,
            ILevelBuilder levelBuilder)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticDataService;
            _levelBuilder = levelBuilder;
        }

        public void Enter(LoadPayload payload)
        {
            _loadPayload = payload;
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(payload.SceneName, OnLoaded);
        }

        public void Exit()
        {
            _levelBuilder.Clear();
        }

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void InitGameWorld()
        {
            if (_loadPayload.IsBuildable == false)
            {
                return;
            }

            Level level = BuildLevel();
            ValidateLevelProgress(level);
            ConstructLevel();
            Vector3 heroPosition = GetHeroPosition();
            GameObject hero = InitHero(heroPosition);
            CameraFollow(hero);
        }

        private Vector3 GetHeroPosition() =>
            _progressService.Progress.WorldData.PositionOnLevel.Position.AsUnityVector();

        private Level BuildLevel() =>
            _levelBuilder.Build(_staticData.ForLevel(_loadPayload.LevelKey));

        private void ConstructLevel() =>
            _levelBuilder.Construct();

        private GameObject InitHero(Vector3 at) =>
            _gameFactory.CreateHero(at);

        private void CameraFollow(GameObject hero)
        {
            if (Camera.main != null) Camera.main.GetComponent<CameraFollower>().Follow(hero.transform);
        }

        private void ValidateLevelProgress(Level level)
        {
            if (_loadPayload.IsBuildable == false)
            {
                return;
            }

            if (_progressService.Progress.WorldData.LevelState.LevelKey == _loadPayload.LevelKey)
            {
                return;
            }

            _progressService.Progress.WorldData.LevelState = new LevelState(_loadPayload.LevelKey);
            _progressService.Progress.WorldData.PositionOnLevel = new PositionOnLevel(_loadPayload.LevelKey, level.HeroInitialPosition.AsVectorData());
            _progressService.Progress.HeroHealthState.MaxHP = _staticData.HealthForEntity(PlayerHealthKey).MaxHealth;
            _progressService.Progress.HeroHealthState.ResetHP();
        }
    }
}