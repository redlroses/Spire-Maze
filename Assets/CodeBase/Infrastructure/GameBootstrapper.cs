using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.SDK;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _curtain;

        private readonly WebInitializer _initializer = new WebInitializer();

        private Game _game;

        private void Awake()
        {
            LoadingCurtain curtain = Instantiate(_curtain);
            _game = new Game(this, curtain);
            _initializer.Start(this, () => _game.StateMachine.Enter<BootstrapState>());

            DontDestroyOnLoad(this);
        }
    }
}