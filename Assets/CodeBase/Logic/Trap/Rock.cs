using CodeBase.Logic.Movement;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(RockMover))]
    [RequireComponent(typeof(RayDirection))]
    public class Rock : Trap
    {
        [SerializeField] private RockMover _mover;
        [SerializeField] private Rigidbody[] _fragments;
        [SerializeField] private RayDirection _rayDirection;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _ground;

        private Transform _selfTransform;
        private bool _isActivated;
        private bool _isNotOnPlate;

        protected override void FixedRun()
        {
            TryDestroy();
        }

        public override void Construct(TrapActivator activator)
        {
            base.Construct(Activator);
            _mover ??= Get<RockMover>();
            _selfTransform = transform;
            _rayDirection ??= Get<RayDirection>();
        }

        protected override void Activate(IDamagable damagable)
        {
            _mover.SetMoveDirection(Activator.transform.position);
            _mover.enabled = true;
            _isActivated = true;
        }

        private void TryDestroy()
        {
            if (_isActivated == false)
                return;

            Vector3 wallDirection = _rayDirection.Calculate(Spire.Position, _selfTransform.localPosition, _mover.Direction);
            Vector3 groundDirection = wallDirection + Vector3.down;

            bool isWallCollision = CheckCollisionObstacle(wallDirection);
            bool isGroundCollision = CheckCollisionObstacle(groundDirection);

            if (isWallCollision || isGroundCollision && _isNotOnPlate)
            {
                Destroy();
            }

            _isNotOnPlate = !isGroundCollision;
        }

        private void Destroy()
        {
            for (int i = 0; i < _fragments.Length; i++)
            {
                _fragments[i].isKinematic = false;
            }
        }

        private bool CheckCollisionObstacle(Vector3 direction)
        {
            Ray ray = new Ray(_selfTransform.localPosition, direction);
            return Physics.Raycast(ray, _rayDistance, _ground);
        }
    }
}