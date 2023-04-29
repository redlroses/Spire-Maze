using UnityEngine;
using CodeBase.Logic.Movement;
using CodeBase.Services.Pause;
using CodeBase.Tools;
using NTC.Global.Cache;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CodeBase.Services.Input
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Jumper))]
    public class PlayerInput : MonoCache, IPauseWatcher
    {
        [SerializeField] [RequireInterface(typeof(IHorizontalMover))]
        private MonoCache _mover;

        [SerializeField] [RequireInterface(typeof(IJumper))]
        private MonoCache _jumper;

        [SerializeField] [RequireInterface(typeof(IDodge))]
        private MonoCache _dodge;

        private InputController _inputController;
        private MoveDirection _direction;
        private IPauseReactive _pauseReactive;

        private IHorizontalMover Mover => (IHorizontalMover) _mover;
        private IJumper Jumper => (IJumper) _jumper;
        private IDodge Dodge => (IDodge) _dodge;

        private void Awake()
        {
            _inputController = new InputController();
        }

        private void OnDestroy()
        {
            _pauseReactive.Pause -= OnPause;
            _pauseReactive.Resume -= OnResume;
        }

        protected override void OnEnabled()
        {
            _inputController.Player.Enable();
            _inputController.Player.Jump.performed += OnJump;
            _inputController.Player.Movement.performed += OnMove;
            _inputController.Player.Movement.canceled += OnMove;
            _inputController.Player.Dodge.performed += OnDodged;
        }

        protected override void OnDisabled()
        {
            _inputController.Player.Disable();
            _inputController.Player.Jump.performed -= OnJump;
            _inputController.Player.Movement.performed -= OnMove;
            _inputController.Player.Movement.canceled -= OnMove;
            _inputController.Player.Dodge.performed -= OnDodged;
        }

        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Pause += OnPause;
            _pauseReactive.Resume += OnResume;
        }

        private void OnResume()
        {
            enabled = true;
        }

        private void OnPause()
        {
            enabled = false;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            int moveInput = Mathf.RoundToInt(context.ReadValue<float>());

            Mover.HorizontalMove((MoveDirection) moveInput);

            if ((MoveDirection) moveInput != MoveDirection.Stop)
            {
                _direction = (MoveDirection) moveInput;
            }
        }

        private void OnJump(InputAction.CallbackContext context) =>
            Jumper.Jump();

        private void OnDodged(InputAction.CallbackContext context)
        {
            Dodge.Evade(_direction);
        }
    }
}