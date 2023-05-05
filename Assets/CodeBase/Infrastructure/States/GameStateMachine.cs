using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.LevelBuild;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.Score;
using CodeBase.Services.Watch;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.States
{
    public sealed class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, AllServices services, ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services, coroutineRunner),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader,
                    services.Single<IGameFactory>(),
                    services.Single<IUIFactory>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<ILevelBuilder>(),
                    services.Single<IScoreService>(),
                    services.Single<IWatchService>(),
                    curtain),
                [typeof(LoadProgressState)] = new LoadProgressState(this,
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>(),
                    services.Single<IStaticDataService>()),
                [typeof(GameLoopState)] = new GameLoopState(this, services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>(), services.Single<IPlayerInputService>(), services.Single<IWatchService>()),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}