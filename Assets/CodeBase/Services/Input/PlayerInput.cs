using UnityEngine;
using CodeBase.Logic.Movement;
using CodeBase.Tools;
using NTC.Global.Cache;

namespace CodeBase.Services.Input
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Jumper))]
    public class PlayerInput : MonoCache
    {
        [SerializeField]
        [RequireInterface(typeof(IMover))] private MonoCache _mover;
        [SerializeField]
        [RequireInterface(typeof(IJumper))] private MonoCache _jumper;

        private InputController _inputController;

        private IMover Mover => (IMover)_mover;
        private IJumper Jumper => (IJumper)_jumper;

        private void Awake()
        {
            _inputController = new InputController();
        }

        protected override void OnEnabled()
        {
            _inputController.Player.Enable();
            _inputController.Player.Jump.performed += context => OnJump();
            _inputController.Player.Movement.performed += context => OnMove();
            _inputController.Player.Movement.canceled += context => OnMove();
        }

        protected override void OnDisabled()
        {
            _inputController.Player.Disable();
            _inputController.Player.Jump.performed -= context => OnJump();
            _inputController.Player.Movement.performed -= context => OnMove();
            _inputController.Player.Movement.canceled -= context => OnMove();
        }

        private void OnMove()
        {
            int MoveInput = Mathf.RoundToInt(_inputController.Player.Movement.ReadValue<float>());
            MoveDiraction diraction = (MoveDiraction)MoveInput;

            Mover.Move(diraction);
        }

        private void OnJump() => Jumper.Jump();
    }
}