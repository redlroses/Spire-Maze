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

        public GameStateMachine(SceneLoader sceneLoader, AllServices services, LoadingCurtain curtain)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(
                    this,
                    services.Single<IADService>(),
                    services.Single<IAnalyticsService>(),
                    services.Single<ICameraOperatorService>(),
                    services.Single<IGameFactory>(),
                    services.Single<IInputService>(),
                    services.Single<IUIFactory>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<ILevelBuilder>(),
                    services.Single<IWatchService>(),
                    services.Single<IPauseService>(),
                    services.Single<IWindowService>(),
                    services.Single<ISoundService>(),
                    sceneLoader,
                    curtain),
                [typeof(LoadProgressState)] = new LoadProgressState(
                    this,
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<ISoundService>()),
                [typeof(GameLoopState)] = new GameLoopState(
                    services.Single<IInputService>(),
                    services.Single<IWatchService>()),
                [typeof(FinishState)] = new FinishState(
                    services.Single<IAnalyticsService>(),
                    services.Single<IGameFactory>() as IHeroLocator,
                    services.Single<IPersistentProgressService>(),
                    services.Single<IRankedService>(),
                    services.Single<ISaveLoadService>(),
                    services.Single<IScoreService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<IWatchService>(),
                    services.Single<IWindowService>()),
            };
        }

        public IExitableState ActiveState => _activeState;

        public void Enter<TState>()
            where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload)
            where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>()
            where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>()
            where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}