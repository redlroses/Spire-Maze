using CodeBase.Logic.Movement;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SimpleBallRotator : MonoCache
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        private MoveDirection _direction;

        public void SetDirection(MoveDirection direction)
        {
            _direction = direction;
        }

        protected override void Run()
        {
            _target.Rotate(-(int) _direction * _speed * Time.deltaTime, 0, 0, Space.Self);
        }
    }
}