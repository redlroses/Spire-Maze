using System;
using UnityEngine;
using CodeBase.Logic.Movement;
using CodeBase.Services.Pause;
using CodeBase.UI.Services.Windows;
using UnityEngine.InputSystem;

namespace CodeBase.Services.Input
{
    public class InputService : IInputService
    {
        private readonly InputController _inputController;
        private readonly IPauseService _pauseService;
        private readonly IWindowService _windowService;

        private MoveDirection _direction = MoveDirection.Left;
        private float _readValue;

        public event Action<MoveDirection> HorizontalMove = _ => { };
        public event Action<Vector2> OverviewMove = _ => { };
        public event Action Jump = () => { };
        public event Action<MoveDirection> Dodge = _ => { };
        public event Action MoveStopped = () => { };

        public InputActionPhase MovementPhase => _inputController.Player.Movement.phase;

        public InputService(IPauseService pauseService, IWindowService windowService)
        {
            _windowService = windowService;
            _pauseService = pauseService;
            _inputController = new InputController();
            Subscribe();
        }

        public void Subscribe()
        {
            _inputController.Player.Jump.performed += OnJump;
            _inputController.Player.Movement.performed += OnMove;
            _inputController.Player.Movement.canceled += OnMove;
            _inputController.Player.Dodge.started += OnDodged;
            _inputController.Player.Pause.started += OnPause;
            _inputController.Overview.ViewTower.performed += OnOverviewMove;
            _inputController.Overview.ViewTower.canceled += OnOverviewMove;
        }

        public void Cleanup()
        {
            _inputController.Player.Jump.performed -= OnJump;
            _inputController.Player.Movement.performed -= OnMove;
            _inputController.Player.Movement.canceled -= OnMove;
            _inputController.Player.Dodge.started -= OnDodged;
            _inputController.Player.Pause.started -= OnPause;
            _inputController.Overview.ViewTower.performed -= OnOverviewMove;
            _inputController.Overview.ViewTower.canceled -= OnOverviewMove;

            DisableOverviewMap();
            DisableMovementMap();
        }

        public void EnableMovementMap() =>
            _inputController.Player.Enable();

        public void EnableOverviewMap() =>
            _inputController.Overview.Enable();

        public void DisableMovementMap()
        {
            MoveStopped.Invoke();
            _inputController.Player.Disable();
        }

        public void DisableOverviewMap() =>
            _inputController.Overview.Disable();

        private void OnMove(InputAction.CallbackContext context)
        {
            float readValue = context.ReadValue<float>();
            int moveInput = Mathf.CeilToInt(Mathf.Abs(readValue)) * (int) Mathf.Sign(readValue);

            if ((MoveDirection) moveInput != MoveDirection.Stop)
                _direction = (MoveDirection) moveInput;

            HorizontalMove.Invoke((MoveDirection) moveInput);
        }

        private void OnJump(InputAction.CallbackContext context) =>
            Jump.Invoke();

        private void OnDodged(InputAction.CallbackContext context) =>
            Dodge.Invoke(_direction);

        private void OnOverviewMove(InputAction.CallbackContext context) =>
            OverviewMove.Invoke(context.ReadValue<Vector2>());

        private void OnPause(InputAction.CallbackContext context)
        {
            _pauseService.SetPause(!_pauseService.IsPause);

            if (_pauseService.IsPause)
                _windowService.Open(WindowId.Pause);
        }
    }
}