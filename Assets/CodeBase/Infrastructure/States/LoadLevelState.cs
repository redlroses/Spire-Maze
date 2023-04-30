﻿using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.StaminaEntity;
using CodeBase.Services.Input;
using CodeBase.Services.LevelBuild;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;
using HeroInventory = CodeBase.Logic.Inventory.HeroInventory;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<LoadPayload>
    {
        private const string PlayerKey = "Player";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly ILevelBuilder _levelBuilder;
        private readonly LoadingCurtain _curtain;
        private readonly IUIFactory _uiFactory;

        private LoadPayload _loadPayload;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory,
            IUIFactory uiFactory,
            IPersistentProgressService progressService, IStaticDataService staticDataService,
            ILevelBuilder levelBuilder, LoadingCurtain curtain)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _progressService = progressService;
            _staticData = staticDataService;
            _levelBuilder = levelBuilder;
            _curtain = curtain;
        }

        public void Enter(LoadPayload payload)
        {
            _curtain.Show();
            _loadPayload = payload;
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(payload.SceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
            _levelBuilder.Clear();
        }

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            ValidateLevelProgress();
            var hero = InitHero();
            CameraFollow(hero);
            InformProgressReaders();
            InitHud(hero);

            _stateMachine.Enter<GameLoopState>();
        }

        // ReSharper disable once InconsistentNaming
        private void InitUIRoot() =>
            _uiFactory.CreateUIRoot();

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

            BuildLevel();
            ConstructLevel();
        }

        private Vector3 GetHeroPosition() =>
            _progressService.Progress.WorldData.PositionOnLevel.Position.AsUnityVector();

        private void BuildLevel() =>
            _levelBuilder.Build(_staticData.ForLevel(_loadPayload.LevelId));

        private void ConstructLevel() =>
            _levelBuilder.Construct();

        private GameObject InitHero()
        {
            Vector3 heroPosition = GetHeroPosition();
            GameObject hero = _gameFactory.CreateHero(heroPosition);
            return hero;
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            hud.GetComponent<Canvas>().worldCamera = Camera.main;
            hud.GetComponentInChildren<HealthBarView>().Construct(hero.GetComponentInChildren<IHealthReactive>());
            hud.GetComponentInChildren<StaminaBarView>().Construct(hero.GetComponentInChildren<IStamina>());
            hud.GetComponentInChildren<InventoryView>().Construct(hero.GetComponent<HeroInventory>());
        }

        private void CameraFollow(GameObject hero)
        {
            if (Camera.main != null)
                Camera.main.GetComponent<CameraFollower>().Follow(hero.transform);
        }

        private void ValidateLevelProgress()
        {
            if (_progressService.Progress.WorldData.LevelState.LevelId == _loadPayload.LevelId &&
                _loadPayload.IsClearLoad == false)
            {
                return;
            }

            ResetProgress();
        }

        private void ResetProgress()
        {
            _progressService.Progress.WorldData.LevelState = new LevelState(_loadPayload.LevelId);
            _progressService.Progress.WorldData.PositionOnLevel =
                new PositionOnLevel(_staticData.ForLevel(_loadPayload.LevelId).HeroInitialPosition.AsVectorData());
            _progressService.Progress.WorldData.HeroHealthState.MaxHP = _staticData.HealthForEntity(PlayerKey).MaxHealth;
            _progressService.Progress.WorldData.HeroHealthState.ResetHP();
            _progressService.Progress.WorldData.HeroStaminaState.MaxValue = _staticData.StaminaForEntity(PlayerKey).MaxStamina;
            _progressService.Progress.WorldData.HeroStaminaState.ResetStamina();
        }
    }
}