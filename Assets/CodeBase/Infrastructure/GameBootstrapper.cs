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
                _webFocusObserver = new WebFocusObserver();
                _game.StateMachine.Enter<BootstrapState>();
            });

            DontDestroyOnLoad(this);
        }

#if !UNITY_EDITOR
        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                _webFocusObserver.Unfocus();
            }
            else
            {
                _webFocusObserver.Focus();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                _webFocusObserver.Focus();
            }
            else
            {
                _webFocusObserver.Unfocus();
            }
        }
#endif
    }
}