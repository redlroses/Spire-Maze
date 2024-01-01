using CodeBase.Services.Input;
using CodeBase.Services.Watch;
using UnityEngine;

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
        }

        public void Enter()
        {
            Debug.Log("GameLoopState Enter");
            _inputService.Subscribe();
            _inputService.EnableMovementMap();
            _watchService.Start();
            Debug.Log("GameLoopState Enter End");
        }
    }
}