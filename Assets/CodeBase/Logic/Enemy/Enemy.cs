﻿using CodeBase.Data;
using CodeBase.Data.CellStates;
using CodeBase.Logic.HealthEntity;
using UnityEngine;
using NTC.Global.Cache;
using CodeBase.Logic.Movement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(SphereCaster))]
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(RayDirection))]
    public class Enemy : MonoCache, IEnemy, ISavedProgress
    {
        [SerializeField] [RequireInterface(typeof(IAccelerable))]
        private MonoCache _mover;

        [SerializeField] private EnemyHealth _health;
        [SerializeField] private SphereCaster _sphereCaster;
        [SerializeField] private RayDirection _rayDirection;
        [SerializeField] private int _damage;
        [SerializeField] private float _rayDistance;
        [SerializeField] private float _rayDistanceToTarget;
        [SerializeField] private LayerMask _ground;

        private const float DelayBetweenDetectTarget = 1f;

        private int _id;
        private Transform _selfTransform;
        private MoveDirection _moveDirection;
        private float _currentDelayBetweenDetectTarget;
        private bool _isDied;

        public int Id => _id;
        private IAccelerable Mover => (IAccelerable)_mover;

        public void Construct(int id)
        {
            _id = id;
            _health ??= Get<EnemyHealth>();
            _rayDirection ??= Get<RayDirection>();
            _selfTransform = transform;
            _moveDirection = MoveDirection.Left;
        }

        protected override void OnEnabled()
        {
            _health.Died += OnDied;
        }

        protected override void OnDisabled()
        {
            _health.Died -= OnDied;
        }

        private void OnDied()
        {
            _isDied = true;
            SetActive(_isDied);
        }

        protected override void FixedRun()
        {
            _moveDirection = GetMoveDirection();
            Mover.Move(_moveDirection);

            if (CanDetectTarget()) DetectTarget<Player.Hero>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamagable player) == false)
            {
                return;
            }

            _currentDelayBetweenDetectTarget = DelayBetweenDetectTarget;
            player.Damage(_damage, DamageType.Single);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            EnemyState cellState = progress.WorldData.LevelState.EnemyStates
                .Find(cell => cell.Id == Id);

            if (cellState == null || cellState.IsDied == false)
            {
                return;
            }

            _isDied = cellState.IsDied;
            SetActive(cellState.IsDied);
        }


        public void UpdateProgress(PlayerProgress progress)
        {
            EnemyState cellState = progress.WorldData.LevelState.EnemyStates
                .Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.EnemyStates.Add((new EnemyState(Id, _isDied)));
            }
            else
            {
                cellState.IsDied = _isDied;
            }
        }

        private MoveDirection GetMoveDirection()
        {
            Vector3 localPosition = _selfTransform.localPosition;
            Vector3 wallDirection = _rayDirection.Calculate(Spire.Position, localPosition,
                _moveDirection);
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

        private bool CanDetectTarget()
        {
            if (_currentDelayBetweenDetectTarget <= 0) return true;

            _currentDelayBetweenDetectTarget -= Time.fixedDeltaTime;
            return false;
        }

        private void DetectTarget<T>() where T : MonoBehaviour
        {
            Vector3 localPosition = _selfTransform.localPosition;
            Vector3 rayDirectionForward = _rayDirection.Calculate(Spire.Position, localPosition,
                _moveDirection);
            Vector3 rayDirectionBackward = _rayDirection.Calculate(Spire.Position, localPosition,
                (MoveDirection)((int)_moveDirection * -1));

            bool isInFront = _sphereCaster.CastSphere(rayDirectionForward, _rayDistanceToTarget, out RaycastHit hit);

            if (isInFront)
            {
                if (hit.collider.TryGetComponent(out T _))
                {
                    Mover.EnableBonusSpeed();
                    return;
                }
            }

            bool isInBack = _sphereCaster.CastSphere(rayDirectionBackward, _rayDistanceToTarget, out hit);

            if (isInBack)
            {
                if (hit.collider.TryGetComponent(out T _))
                {
                    Mover.EnableBonusSpeed();
                    ChangeDirection();
                    return;
                }
            }

            Mover.DisableBonusSpeed();
        }

        private void ChangeDirection() => _moveDirection = (MoveDirection)((int)_moveDirection * -1);

        private void SetActive(bool isDied) => gameObject.SetActive(!isDied);
    }
}