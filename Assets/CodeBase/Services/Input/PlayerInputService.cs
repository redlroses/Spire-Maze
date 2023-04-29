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
        // [SerializeField] [RequireInterface(typeof(IHorizontalMover))]
        // private MonoCache _mover;
        //
        // [SerializeField] [RequireInterface(typeof(IJumper))]
        // private MonoCache _jumper;
        //
        // [SerializeField] [RequireInterface(typeof(IDodge))]
        // private MonoCache _dodge;

        private readonly InputController _inputController;
        private readonly IPauseService _pauseService;

        private MoveDirection _direction;
        private IWindowService _windowService;

        // private IHorizontalMover Mover => (IHorizontalMover) _mover;
        // private IJumper Jumper => (IJumper) _jumper;
        // private IDodge Dodge => (IDodge) _dodge;

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

        public void ClearUp()
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
            // Mover.HorizontalMove((MoveDirection) moveInput);

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