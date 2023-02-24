﻿using UnityEngine;
using NTC.Global.Cache;
using CodeBase.Logic.Movement;
using CodeBase.Tools;
using CodeBase.Tools.PhysicsDebug;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(Mover))]
    public class Enemy : MonoCache, IEnemy
    {
        [SerializeField] [RequireInterface(typeof(IAccelerable))]
        private MonoCache _mover;

        [SerializeField] private int _damage;
        [SerializeField] private float _rayDistance;
        [SerializeField] private float _rayDistanceToTarget;
        [SerializeField] private LayerMask _ground;

        private const float DelayBetweenDetectTarget = 1f;

        private Transform _selfTransform;
        private MoveDirection _moveDirection;
        private float _currentDelayBetweenDetectTarget;
        private bool _isRotationToTarget;
        private bool _isDie;

        private IAccelerable Mover => (IAccelerable) _mover;

        private void Awake()
        {
            _selfTransform = transform;
            _moveDirection = MoveDirection.Left;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player) == false)
            {
                return;
            }

            _currentDelayBetweenDetectTarget = DelayBetweenDetectTarget;
            player.ReceiveDamage(_damage);
        }

        protected override void FixedRun()
        {
            Mover.Move(GetMoveDirection());

            if (CanDetectTarget())
                DetectTarget<Player>();
        }

        public void Initialize()
        {
        }

        private MoveDirection GetMoveDirection()
        {
            Vector3 localPosition = _selfTransform.localPosition;
            Vector3 wallDirection = CalculateRayDirection(Spire.Position, localPosition, _moveDirection);
            Vector3 groundDirection = wallDirection + Vector3.down;

            bool isWall = CheckForDirection(wallDirection);
            bool isNotGround = !CheckForDirection(groundDirection);

            if (isWall || isNotGround)
            {
                ChangeDirection();
            }

            return _moveDirection;
        }

        private bool CheckForDirection(Vector3 direction)
        {
            Ray ray = new Ray(_selfTransform.localPosition, direction);
            return Physics.Raycast(ray, _rayDistance, _ground);
        }

        private Vector3 CalculateRayDirection(Vector3 anchorPoint, Vector3 currentPoint, MoveDirection direction)
        {
            Vector3 directionForAnchor = new Vector3(anchorPoint.x, currentPoint.y, anchorPoint.z) - currentPoint;
            return Vector3.Cross(directionForAnchor, Vector3.down * (int) direction).normalized;
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
            Vector3 localPosition = _selfTransform.localPosition;
            Vector3 rayDirectionForward =
                CalculateRayDirection(Spire.Position, localPosition, MoveDirection.Right);
            Vector3 rayDirectionBackward =
                CalculateRayDirection(Spire.Position, localPosition, MoveDirection.Left);
            Ray rayForward = new Ray(localPosition, rayDirectionForward);
            Ray rayBackward = new Ray(localPosition, rayDirectionBackward);

            if (Physics.Raycast(rayForward, out RaycastHit hit, _rayDistanceToTarget)
                || Physics.Raycast(rayBackward, out hit, _rayDistanceToTarget))

                if (hit.collider.TryGetComponent(out T _))
                {
                    Mover.EnableBonusSpeed();

                    if (Vector3.Dot(_selfTransform.forward.normalized,
                        (hit.transform.position - _selfTransform.position).normalized) < 0 == false) return;
                    if (_isRotationToTarget)
                        return;

                    ChangeDirection();

                    _isRotationToTarget = true;

                    return;
                }
                else
                {
                    Mover.DisableBonusSpeed();
                }

            _isRotationToTarget = false;
        }

        private void ChangeDirection() => _moveDirection = (MoveDirection) ((int) _moveDirection * -1);
    }
}