using UnityEngine;
using CodeBase.Tools.Extension;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoCache, IMover
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody _rigidbody;

        protected float Speed => _speed;
        public Rigidbody Rigidbody => _rigidbody;

        private MoveDirection _direction;

        private void Awake()
        {
            _rigidbody ??= Get<Rigidbody>();
        }

        protected override void FixedRun()
        {
            ApplyMove();
        }

        public void Move(MoveDirection direction) => _direction = direction;

        protected virtual float CalculateSpeed() =>
            _speed;

        private void ApplyMove()
        {
            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);

            if (_direction == MoveDirection.Stop)
                return;

            Vector2 direction = new Vector2((int) _direction, 0f);
            Vector3 velocity = direction.ToWorldDirection(_rigidbody.position, Spire.DistanceToCenter) * CalculateSpeed();

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            _rigidbody.MoveRotation(targetRotation);

            velocity.ChangeY(_rigidbody.velocity.y);
            _rigidbody.velocity = velocity.ChangeY(_rigidbody.velocity.y);
        }
    }
}