using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.SDK;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private readonly WebSDKInitializer _initializer = new WebSDKInitializer();

        [SerializeField] private LoadingCurtain _curtain;

        private Game _game;
        private WebFocusObserver _webFocusObserver;

        private void Awake()
        {
            LoadingCurtain curtain = Instantiate(_curtain);
            _game = new Game(this, curtain);
            _initializer.Start(this, () =>
            {
                _game.StateMachine.Enter<BootstrapState>();
                _webFocusObserver = new WebFocusObserver();
            });

            DontDestroyOnLoad(this);
        }

#if !UNITY_EDITOR
        private void OnApplicationFocus(bool hasFocus) =>
            _webFocusObserver?.UpdateFocus(!hasFocus);
#endif
    }
}