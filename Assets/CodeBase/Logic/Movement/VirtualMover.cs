using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class VirtualMover : MonoCache
    {
        [SerializeField] private float _speed = 10f;

        private Vector2 _direction;
        private Vector2 _heightRange;
        private float _radius;

        public void Construct(Vector2 heightRange)
        {
            _heightRange = heightRange;
            _radius = transform.position.RemoveY().magnitude;
        }

        public void Move(Vector2 direction)
        {
            enabled = direction.sqrMagnitude > 0;
            _direction = direction;
        }

        protected override void Run()
        {
            Transform selfTransform = transform;
            Vector3 worldDirection = _direction.ToWorldDirection(selfTransform.position, _radius).ChangeY(_direction.y);
            transform.Translate(worldDirection.normalized * _speed * Time.deltaTime);

            if (selfTransform.position.y > _heightRange.y)
            {
                selfTransform.position = selfTransform.position.ChangeY(_heightRange.y);
            }
            else if (selfTransform.position.y < _heightRange.x)
            {
                selfTransform.position = selfTransform.position.ChangeY(_heightRange.x);
            }
        }
    }
}