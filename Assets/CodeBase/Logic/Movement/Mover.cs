using UnityEngine;
using CodeBase.Data;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoCache, IMover
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _bonusSpeed;
        [SerializeField] private float _durationBonusSpeed;

        private Rigidbody _rigidbody;
        private MoveDirection _direction;
        private float _timerBonusSpeed;
        private bool _isBonusSpeedEnabled;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected override void FixedRun()
        {
            ApplyMove();

            if (_timerBonusSpeed <= 0)
                DisableBonusSpeed();
            else
                _timerBonusSpeed -= Time.fixedDeltaTime;
        }

        public void Move(MoveDirection direction) => _direction = direction;

        public void EnableBonusSpeed()
        {
            if (_isBonusSpeedEnabled)
                return;

            _speed += _bonusSpeed;
            _isBonusSpeedEnabled = true;
            _timerBonusSpeed = _durationBonusSpeed;
        }

        public void DisableBonusSpeed()
        {
            if (_isBonusSpeedEnabled == false)
                return;

            _speed -= _bonusSpeed;
            _isBonusSpeedEnabled = false;
        }

        private void ApplyMove()
        {
            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);

            if (_direction == MoveDirection.Stop)
                return;

            Vector3 velocity = CalculateVelocity(Spire.Position, _rigidbody.position, _direction) * _speed;
            _rigidbody.velocity = CorrectVelocity(velocity.ExcludeAxisY(), _rigidbody.position.ExcludeAxisY());

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            _rigidbody.MoveRotation(targetRotation);
        }

        private Vector3 CalculateVelocity(Vector3 anchorPoint, Vector3 currentPoint, MoveDirection direction)
        {
            Vector3 directionForAnchor = new Vector3(anchorPoint.x, currentPoint.y, anchorPoint.z) - currentPoint;

            return Vector3.Cross(directionForAnchor, Vector3.down * (int) direction).normalized;
        }

        private Vector3 CorrectVelocity(Vector2 currentVelocity, Vector2 currentPosition)
        {
            Vector2 uncorrectedNextPosition = currentPosition + currentVelocity * Time.fixedDeltaTime;
            Vector2 pointOnArc = uncorrectedNextPosition.normalized * Spire.DistanceToCenter;
            Vector2 correctedVelocity = (pointOnArc - currentPosition).normalized * _speed;

            return new Vector3(correctedVelocity.x, _rigidbody.velocity.y, correctedVelocity.y);
        }
    }
}