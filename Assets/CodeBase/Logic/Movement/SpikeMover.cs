using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class SpikeMover : MonoCache
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _distanceY;
        [SerializeField] private float _speed;

        private float _targetPositionY;

        private void Awake()
        {
            _rigidbody ??= Get<Rigidbody>();
            _targetPositionY = _rigidbody.position.y + _distanceY;
        }

        protected override void FixedRun()
        {
            Move();
        }

        private void Move()
        {
            if (_rigidbody.position.y >= _targetPositionY)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            float positionY = _rigidbody.position.y * _speed * Time.fixedDeltaTime;

            _rigidbody.velocity = _rigidbody.velocity.ChangeY(positionY);
        }
    }
}