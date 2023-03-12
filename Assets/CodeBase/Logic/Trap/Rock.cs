using CodeBase.Logic.Movement;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(RockMover))]

    public class Rock : Trap
    {
        [SerializeField] private RockMover _mover;
        [SerializeField] private Rigidbody[] _fragments;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _ground;

        private Transform _selfTransform;
        private bool _isActivated;
        private bool _isNotOnPlate;

        private void Awake()
        {
            _mover ??= Get<RockMover>();
            _selfTransform = transform;
        }

        protected override void FixedRun()
        {
            TryDestroy();
        }

        protected override void Activate()
        {
            _mover.SetMoveDirection(Activator.transform.position);
            _mover.enabled = true;
            _isActivated = true;
        }

        protected void TryDestroy()
        {
            if (_isActivated == false)
                return;

            Vector3 wallDirection = CalculateRayDirection(Spire.Position, _selfTransform.localPosition, _mover.Direction);
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

        private Vector3 CalculateRayDirection(Vector3 anchorPoint, Vector3 currentPoint, MoveDirection direction)
        {
            Vector3 directionForAnchor = new Vector3(anchorPoint.x, currentPoint.y, anchorPoint.z) - currentPoint;
            return Vector3.Cross(directionForAnchor, Vector3.down * (int)direction).normalized;
        }

        private bool CheckCollisionObstacle(Vector3 direction)
        {
            Ray ray = new Ray(_selfTransform.localPosition, direction);
            return Physics.Raycast(ray, _rayDistance, _ground);
        }
    }
}