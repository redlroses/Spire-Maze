using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class VirtualMover : MonoCache
    {
        [SerializeField] private float _speed = 10f;

        private Vector2 _direction;

        public void Move(Vector2 direction)
        {
            enabled = direction.sqrMagnitude > 0;
            _direction = direction;
        }

        protected override void Run()
        {
            Vector3 worldDirection = _direction.ToWorldDirection(transform.position, Spire.DistanceToCenter).ChangeY(_direction.y);
            transform.Translate(worldDirection.normalized * _speed * Time.deltaTime);
        }
    }
}