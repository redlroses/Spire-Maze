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
        [SerializeField] private float _rayDistanceToTarget;
        [SerializeField] private LayerMask _ground;

        private const float DelayBetweenDetectTarget = 1f;

        private MoveDirection _moveDirection;
        private float _currentDelayBetweenDetectTarget;
        private bool _isDie;
        private bool _isRotationToTarget;

        public Mover Mover => (Mover)_mover;

        private void Awake()
        {
            _moveDirection = MoveDirection.Left;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
                _currentDelayBetweenDetectTarget = DelayBetweenDetectTarget;
        }

        protected override void FixedRun()
        {
            Move();

            if (CanDetectTarget())
                DetectTarget<Player>();
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
            Ray ray = new Ray(transform.localPosition, direction + Vector3.down);

            if (Physics.Raycast(ray, _rayDistance, _ground))
                return _moveDirection;

            return ChangeDirection();
        }

        private Vector3 CalculateRayDirection(Vector3 anchorPoint, Vector3 currentPoint, MoveDirection direction)
        {
            Vector3 directionForAnchor = new Vector3(anchorPoint.x, currentPoint.y, anchorPoint.z) - currentPoint;

            return Vector3.Cross(directionForAnchor, Vector3.down * (int)direction).normalized;
        }

        private bool CanDetectTarget()
        {
            if (_currentDelayBetweenDetectTarget <= 0)
                return true;

            _currentDelayBetweenDetectTarget -= Time.fixedDeltaTime;
            return false;
        }

        private void DetectTarget<T>() where T : MonoCache
        {
            Vector3 rayDirectionForward = CalculateRayDirection(_spire.position, transform.localPosition, MoveDirection.Right);
            Vector3 rayDirectionBackward = CalculateRayDirection(_spire.position, transform.localPosition, MoveDirection.Left);
            Ray rayForward = new Ray(transform.localPosition, rayDirectionForward);
            Ray rayBackward = new Ray(transform.localPosition, rayDirectionBackward);
            RaycastHit hit;

            if (Physics.Raycast(rayForward, out hit, _rayDistanceToTarget)
                || Physics.Raycast(rayBackward, out hit, _rayDistanceToTarget))
                if (hit.collider.TryGetComponent(out T _))
                {
                    
                    Mover.EnableBonusSpeed();

                    if (Vector3.Dot(transform.forward.normalized, (hit.transform.position - transform.position).normalized) < 0)
                    {
                        if (_isRotationToTarget == true)
                            return;

                        ChangeDirection();
                        
                        _isRotationToTarget = true;
                    }

                    return;
                }

            _isRotationToTarget = false;
        }

        private MoveDirection ChangeDirection() => _moveDirection = _moveDirection == MoveDirection.Right ? MoveDirection.Left : MoveDirection.Right;
    }
}
