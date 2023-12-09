using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.ADS;
using CodeBase.Services.Analytics;
using CodeBase.Services.Cameras;
using CodeBase.Services.Input;
using CodeBase.Services.LevelBuild;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Ranked;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Score;
using CodeBase.Services.Sound;
using CodeBase.Services.StaticData;
using CodeBase.Services.Watch;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.States
{
    public sealed class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, AllServices services,
            LoadingCurtain curtain)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader,
                    services.Single<IGameFactory>(),
                    services.Single<IInputService>(),
                    services.Single<IUIFactory>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<ILevelBuilder>(),
                    services.Single<IWatchService>(),
                    services.Single<IPauseService>(),
                    services.Single<IAnalyticsService>(),
                    services.Single<ICameraOperatorService>(),
                    services.Single<IWindowService>(),
                    services.Single<IADService>(),
                    curtain),
                [typeof(LoadProgressState)] = new LoadProgressState(this,
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<ISoundService>()),
                [typeof(GameLoopState)] = new GameLoopState(services.Single<IInputService>(),
                    services.Single<IWatchService>()),
                [typeof(FinishState)] = new FinishState(
                    services.Single<IWindowService>(),
                    services.Single<IScoreService>(),
                    services.Single<IRankedService>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<IWatchService>(),
                    services.Single<IGameFactory>() as IHeroLocator,
                    services.Single<IStaticDataService>(),
                    services.Single<IAnalyticsService>())
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}