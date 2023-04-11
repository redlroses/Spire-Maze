using UnityEngine;
using CodeBase.Tools.Extension;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoCache, IMover
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private Rigidbody _rigidbody;

        protected MoveDirection Direction;
        
        protected float Speed => _speed;
        public Rigidbody Rigidbody => _rigidbody;

        private void Awake()
        {
            _rigidbody ??= Get<Rigidbody>();
            ApplyMove(MoveDirection.Left);
        }

        protected override void FixedRun()
        {
            ApplyMove(Direction);
        }

        public void Move(MoveDirection direction) => Direction = direction;

        protected virtual float CalculateSpeed() =>
            _speed;

        private void ApplyMove(MoveDirection moveDirection)
        {
            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);

            if (moveDirection == MoveDirection.Stop)
                return;

            Vector2 direction = new Vector2((int) moveDirection, 0f);
            Vector3 velocity = direction.ToWorldDirection(_rigidbody.position, Spire.DistanceToCenter) * CalculateSpeed();

            Quaternion lookRotation = Quaternion.LookRotation(velocity);
            Quaternion targetRotation = Quaternion.Slerp(_rigidbody.rotation, lookRotation, _rotateSpeed * Time.fixedDeltaTime);

            _rigidbody.MoveRotation(targetRotation);
            _rigidbody.velocity = velocity.ChangeY(_rigidbody.velocity.y);
        }
    }
}