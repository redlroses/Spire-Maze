using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.Logic;

namespace CodeBase.Infrastructure
{
    public sealed class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner, curtain), AllServices.Container, curtain);
        }
    }
}