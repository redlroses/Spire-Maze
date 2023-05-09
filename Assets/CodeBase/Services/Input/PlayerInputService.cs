using System;
using UnityEngine;
using CodeBase.Logic.Movement;
using CodeBase.Services.Pause;
using CodeBase.UI.Services.Windows;
using UnityEngine.InputSystem;

namespace CodeBase.Services.Input
{
    public class PlayerInputService : IPlayerInputService
    {
        private readonly InputController _inputController;
        private readonly IPauseService _pauseService;
        private readonly IWindowService _windowService;

        private MoveDirection _direction;

        public event Action<MoveDirection> HorizontalMove;
        public event Action Jump;
        public event Action<MoveDirection> Dodge;

        public PlayerInputService(IPauseService pauseService, IWindowService windowService)
        {
            _windowService = windowService;
            _pauseService = pauseService;
            _inputController = new InputController();
            Subscribe();
        }

        public void Subscribe()
        {
            _inputController.Player.Enable();
            _inputController.Player.Jump.performed += OnJump;
            _inputController.Player.Movement.performed += OnMove;
            _inputController.Player.Movement.canceled += OnMove;
            _inputController.Player.Dodge.performed += OnDodged;
            _inputController.Player.Pause.started += OnPause;
        }

        public void Cleanup()
        {
            _inputController.Player.Disable();
            _inputController.Player.Jump.performed -= OnJump;
            _inputController.Player.Movement.performed -= OnMove;
            _inputController.Player.Movement.canceled -= OnMove;
            _inputController.Player.Dodge.performed -= OnDodged;
            _inputController.Player.Pause.started -= OnPause;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            int moveInput = Mathf.RoundToInt(context.ReadValue<float>());

            HorizontalMove?.Invoke((MoveDirection) moveInput);

            if ((MoveDirection) moveInput != MoveDirection.Stop)
            {
                _direction = (MoveDirection) moveInput;
            }
        }

        private void OnJump(InputAction.CallbackContext context) =>
            Jump?.Invoke();

        private void OnDodged(InputAction.CallbackContext context) =>
            Dodge?.Invoke(_direction);

        private void OnPause(InputAction.CallbackContext context)
        {
            _pauseService.SetPause(!_pauseService.IsPause);

            if (_pauseService.IsPause)
            {
                _windowService.Open(WindowId.Pause);
            }
        }
    }
}