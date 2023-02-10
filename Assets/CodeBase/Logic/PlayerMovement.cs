using UnityEngine;
using UnityEngine.InputSystem;
using CodeBase.Services.Input;
using NTC.Global.Cache;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InteractionHandler))]
    public class PlayerMovement : MonoCache
    {
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _speed;
        [SerializeField] private Transform _spire;

        private const float DistanceForSpire = 6f;
        private float _offsetLookAtPoint = 0.01f;

        private Rigidbody _rigidbody;
        private InteractionHandler _interactionHandler;
        private InputController _inputController;
        private float _angle;
        private bool _isMove;
        private bool _isMoveToRight;
        private bool _isGrounded = true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _interactionHandler = GetComponent<InteractionHandler>();
            _inputController = new InputController();
        }

        protected override void OnEnabled()
        {
            _interactionHandler.PlacedPlates += OnPlacedPlates;
            _inputController.Player.Enable();
            _inputController.Player.Jump.performed += OnJump;
        }

        protected override void OnDisabled()
        {
            _interactionHandler.PlacedPlates -= OnPlacedPlates;
            _inputController.Player.Disable();
            _inputController.Player.Jump.performed -= OnJump;
        }

        protected override void FixedRun()
        {
            Move();
            LookAhead(_rigidbody.position, _isMoveToRight);
        }

        private void Move()
        {
            float MoveInput = _inputController.Player.Movement.ReadValue<float>();
            _isMove = MoveInput != 0f;

            if (_isMove == false)
                return;

            float positionX = _spire.position.x + Mathf.Cos(_angle) * DistanceForSpire;
            float positionZ = _spire.position.z + Mathf.Sin(_angle) * DistanceForSpire;
            Vector3 direction = new Vector3(positionX, _rigidbody.position.y, positionZ);

            _rigidbody.MovePosition(direction);
            _isMoveToRight = MoveInput > 0f;

            _angle += MoveInput * Time.fixedDeltaTime * _speed;

            if (_angle >= 360)
                _angle = 0f;
        }

        private void LookAhead(Vector3 curretnPosition, bool isMoveToRight)  //TODO: Наблюдаются фризы при повороте влево; 
        {
            if (_isMove == false)
                return;

            _offsetLookAtPoint = isMoveToRight == true ? _offsetLookAtPoint : -_offsetLookAtPoint;

            Vector3 lookAtPoint = new Vector3(curretnPosition.x + _offsetLookAtPoint, _rigidbody.position.y, curretnPosition.z + _offsetLookAtPoint);
            Vector3 directionRotate = (lookAtPoint - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(directionRotate);

            _rigidbody.MoveRotation(rotation);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && _isGrounded == true)
            {
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _isGrounded = false;
            }
        }

        private void OnPlacedPlates() => _isGrounded = true;
    }
}