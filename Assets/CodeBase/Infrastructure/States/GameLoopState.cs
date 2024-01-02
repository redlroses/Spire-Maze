using CodeBase.Services.Input;
using CodeBase.Services.Watch;

namespace CodeBase.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly IInputService _inputService;
        private readonly IWatchService _watchService;

        public GameLoopState(IInputService inputService, IWatchService watchService)
        {
            _inputService = inputService;
            _watchService = watchService;
        }

        public void Exit()
        {
            _inputService.Cleanup();
            _watchService.Stop();

#if CRAZY_GAMES
            CrazyGames.CrazyEvents.Instance.GameplayStop();
#endif
        }

        public void Enter()
        {
            _inputService.Subscribe();
            _inputService.EnableMovementMap();
            _watchService.Start();

#if CRAZY_GAMES
            CrazyGames.CrazyEvents.Instance.GameplayStart();
#endif
        }
    }
}