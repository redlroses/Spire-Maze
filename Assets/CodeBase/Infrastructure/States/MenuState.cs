using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Elements.Buttons.TransitionButtons;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class MenuState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;

        public MenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter()
        {
            _sceneLoader.Load(LevelNames.Menu, OnLoaded);
        }

        private void OnLoaded()
        {
            GameObject menu = _gameFactory.CreateMenu();
            menu.GetComponentInChildren<PlayButton>().Construct(_gameStateMachine, _progressService);
            _gameFactory.CreateMusicPlayer();
#if !UNITY_EDITOR && YANDEX_GAMES
            Agava.YandexGames.YandexGamesSdk.GameReady();
#endif
        }

        public void Exit()
        {
        }
    }
}