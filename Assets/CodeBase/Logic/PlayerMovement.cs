using UnityEngine;
using UnityEngine.InputSystem;
using CodeBase.Services.Input;
using NTC.Global.Cache;
using System;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InteractionHandler))]
    public class PlayerMovement : MonoCache
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform _spire;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _durationJamp = 1;

        private const float DistanceForSpire = 6f;
        private float _offsetLookAtPoint = 0.01f;

        private Rigidbody _rigidbody;
        private InteractionHandler _interactionHandler;
        private InputController _inputController;
        [SerializeField] private float _angle;
        private float _expiredTime;
        private float _jumpProgress;
        private bool _isMove;
        private bool _isJump;
        private bool _isMoveToRight;
        private bool _isPlate = true;


        private Vector3 _pointOnArc;
        private Vector3 _correctedVelocity;


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

        //private void OnValidate()
        //{
        //    float cosB = (transform.position.x - _spire.position.x) / DistanceForSpire;
        //    float B = Mathf.Acos(cosB);
        //    Debug.Log(B);
        //    float positionX = _spire.position.x + Mathf.Cos(_angle + B) * DistanceForSpire;
        //    float positionZ = _spire.position.z + Mathf.Sin(_angle + B) * DistanceForSpire;
        //    Vector3 direction = new Vector3(positionX, transform.position.y, positionZ);

        //    transform.position = direction;

        //    float distance = new Vector2(6f - transform.position.x, 0 - transform.position.z).magnitude;
        //    //    Debug.Log(distance*360/(2*Mathf.PI*6f));
        //}

        private void OnJump(InputAction.CallbackContext context)
        {
            if (_isPlate == false)
                return;

            _isJump = true;
        }

        protected override void FixedRun()
        {
            Move();
            Jump();
            //   LookAhead(_rigidbody.position, _isMoveToRight);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
          //  Gizmos.DrawLine(transform.position, _pointOnArc);
        }

        private void Move()
        {
            float MoveInput = _inputController.Player.Movement.ReadValue<float>();
            _isMove = MoveInput != 0f;

            if (_isMove == false)
                return;

            var radiusDirection = new Vector3(_spire.position.x, _rigidbody.position.y, _spire.position.z) - _rigidbody.position;
            var moveDirection = Vector3.Cross(radiusDirection, Vector3.down * MoveInput).normalized * _speed ;

            //Debug.Log($"Radius direction: {radiusDirection}");
            //Debug.Log($"move Direction: {moveDirection}");


            Debug.Log($"Velocity befor correction: {moveDirection}");
            Vector3 velocity = CorrectSpeed(moveDirection, _rigidbody.position);
            Debug.Log($"Velocity after correction: {velocity}");

            velocity = new Vector3(velocity.z,_rigidbody.velocity.y,-velocity.x);
            _rigidbody.velocity = velocity;
            _isMoveToRight = MoveInput > 0f;


            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _rigidbody.MoveRotation(targetRotation);
        }

        private Vector3 CorrectSpeed(Vector3 currentVelocity, Vector3 currentPosition)
        {
            currentPosition.y = 0f;
            currentVelocity.y = 0f;
            Vector3 uncorrectedNextPosition = currentPosition + currentVelocity * Time.fixedDeltaTime;
            _pointOnArc = (uncorrectedNextPosition).normalized * DistanceForSpire;
            _pointOnArc.y = _rigidbody.position.y;
           // Debug.Log($"pointOnArc: {_pointOnArc}, {_pointOnArc.magnitude}");
            _correctedVelocity = (_pointOnArc * Time.fixedDeltaTime - currentPosition).normalized * _speed;
     //       Debug.Log($"correctedVelocity: {_correctedVelocity}");
            return _correctedVelocity;
        }

        //private void Move()
        //{
        //    float MoveInput = _inputController.Player.Movement.ReadValue<float>();
        //    _isMove = MoveInput != 0f;

        //    if (_isMove == false)
        //        return;

        //    float positionX = _spire.position.x + Mathf.Cos(_angle) * DistanceForSpire;
        //    float positionZ = _spire.position.z + Mathf.Sin(_angle) * DistanceForSpire;
        //    Vector3 direction = new Vector3(positionX, _rigidbody.position.y, positionZ);

        //    _rigidbody.MovePosition(direction);
        //    _isMoveToRight = MoveInput > 0f;

        //    _angle += MoveInput * Time.fixedDeltaTime * _speed;
        //}

        private void LookAhead(Vector3 curretnPosition, bool isMoveToRight)
        {
            if (_isMove == false)
                return;

            _offsetLookAtPoint = isMoveToRight == true ? _offsetLookAtPoint : -_offsetLookAtPoint;

            Vector3 lookAtPoint = new Vector3(curretnPosition.x + _offsetLookAtPoint, _rigidbody.position.y, curretnPosition.z + _offsetLookAtPoint);
            Vector3 directionRotate = (lookAtPoint - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(directionRotate);

            _rigidbody.MoveRotation(rotation);
        }

        private void Jump()
        {
            if (_isJump == false)
                return;

            _expiredTime += Time.deltaTime;

            if (_expiredTime > _durationJamp)
            {
                _expiredTime = 0;
                _isJump = false;
            }

            _jumpProgress = _expiredTime / _durationJamp;

            _rigidbody.MovePosition(_rigidbody.position + new Vector3(0, _jumpCurve.Evaluate(_jumpProgress) * _jumpForce, 0));
            _isPlate = false;
        }

        private void OnPlacedPlates() => _isPlate = true;
    }
}