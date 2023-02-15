using UnityEngine;
using CodeBase.Data;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoCache, IMover
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform _spire;

        private const float DistanceForSpire = 6f;

        private Rigidbody _rigidbody;
        private MoveDiraction _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected override void FixedRun()
        {
            ApplyMove();
        }

        public void Move(MoveDiraction direction) => _direction = direction;

        private void ApplyMove()
        {
            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);

            if (_direction == MoveDiraction.Stop)
                return;

            Vector3 velocity = CalculateVelocity(_spire.position, _rigidbody.position, _direction) * _speed;
            _rigidbody.velocity = CorrectVelocity(velocity.ExcludeAxisY(), _rigidbody.position.ExcludeAxisY());

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            _rigidbody.MoveRotation(targetRotation);
        }

        private Vector3 CalculateVelocity(Vector3 anchorPoint, Vector3 currentPoint, MoveDiraction direction)
        {
            Vector3 directionForAnchor = new Vector3(anchorPoint.x, currentPoint.y, anchorPoint.z) - currentPoint;

            return Vector3.Cross(directionForAnchor, Vector3.down * (int)direction).normalized;
        }

        private Vector3 CorrectVelocity(Vector2 currentVelocity, Vector2 currentPosition)
        {
            Vector2 uncorrectedNextPosition = currentPosition + currentVelocity * Time.fixedDeltaTime;
            Vector2 pointOnArc = uncorrectedNextPosition.normalized * DistanceForSpire;
            Vector2 correctedVelocity = (pointOnArc - currentPosition).normalized * _speed;

            return new Vector3(correctedVelocity.x, _rigidbody.velocity.y, correctedVelocity.y);
        }
    }
}