using UnityEngine;
using NTC.Global.Cache;
using CodeBase.Logic.Movement;
using CodeBase.Tools;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(Mover))]
    public class Enemy : MonoCache, IEnemy
    {
        [SerializeField]
        [RequireInterface(typeof(Mover))] private MonoCache _mover;
        [SerializeField] private Transform _spire;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _ground;

        private MoveDirection _moveDirection;
        private bool _isDie;

        public Mover Mover => (Mover)_mover;

        private void Awake()
        {
            _moveDirection = MoveDirection.Left;
        }

        protected override void FixedRun()
        {
            Move();
        }

        public void Initialize()
        {

        }

        private void Move()
        {
            Mover.Move(GetMoveDirection());
        }

        private MoveDirection GetMoveDirection()
        {
            Vector3 direction = CalculateRayDirection(_spire.position, transform.localPosition, _moveDirection);
            Ray ray = new Ray(transform.localPosition, direction);

            if (Physics.Raycast(ray, _rayDistance, _ground))
                return _moveDirection;

            return _moveDirection = _moveDirection == MoveDirection.Right ? MoveDirection.Left : MoveDirection.Right;
        }

        private Vector3 CalculateRayDirection(Vector3 anchorPoint, Vector3 currentPoint, MoveDirection direction)
        {
            Vector3 directionForAnchor = new Vector3(anchorPoint.x, currentPoint.y, anchorPoint.z) - currentPoint;

            return Vector3.Cross(directionForAnchor, Vector3.down * (int)direction).normalized + Vector3.down;
        }
    }
}