using CodeBase.Data;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Movement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools;
using CodeBase.Tools.Extension;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(RockMover))]
    [RequireComponent(typeof(RayDirection))]
    public class Rock : Trap, ISavedProgress, IIndexable
    {
        [SerializeField] private CapsuleCollider _collisionCollider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private RockMover _mover;
        [SerializeField] private SimpleBallRotator _rotator;
        [SerializeField] private Rigidbody[] _fragments;
        [SerializeField] private RayDirection _rayDirection;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _ground;
        [SerializeField] private LayerMask _wall;
        [SerializeField] private ParticleSystem _destroyEffect;
        [SerializeField] private ParticleStopCallback _stopEffectCallback;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _timerDelay;

        private Transform _selfTransform;
        private MoveDirection _direction;
        private bool _isNotOnPlate;
        private bool _hasTargetRotationReached;

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        protected override void FixedRun()
        {
            TryDestroy();
            Rotate();
        }

        public void Construct(int id, TrapActivator activator)
        {
            base.Construct(activator);
            Id = id;
            _rigidbody ??= Get<Rigidbody>();
            _mover ??= Get<RockMover>();
            _mover.enabled = false;
            _selfTransform = transform;
            _rayDirection ??= Get<RayDirection>();
        }

        public void SetMoveDirection(bool isDirectionToRight)
        {
            _direction = isDirectionToRight ? MoveDirection.Right : MoveDirection.Left;
            Rotate();
            _rotator.SetDirection(_direction);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables
                .Find(cell => cell.Id == Id);

            if (cellState == null || cellState.IsActivated == false)
            {
                return;
            }

            IsActivated = cellState.IsActivated;

            if (cellState.IsActivated)
            {
                OnDestroyGameObject();
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables
                .Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.Indexables.Add(new IndexableState(Id, IsActivated));
            }
            else
            {
                cellState.IsActivated = IsActivated;
            }
        }

        protected override void Activate(IDamagable damagable)
        {
            _mover.Move(_direction);
            _mover.Enable();
            _rotator.Enable();
            _timer.SetUp(_timerDelay, OnTurnOffFragments);
            _stopEffectCallback.SetCallback(OnDestroyGameObject);
            IsActivated = true;
        }

        private void Rotate()
        {
            Vector2 direction = new Vector2((int) _direction, 0f);
            Vector3 moveDirection = direction.ToWorldDirection(transform.parent.position, Spire.DistanceToCenter);
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);

            _selfTransform.parent.rotation = lookRotation;
        }

        private void TryDestroy()
        {
            if (IsActivated == false)
                return;

            Vector3 wallDirection = _rayDirection.Calculate(Spire.Position, _selfTransform.position, _direction);
            Vector3 groundDirection = wallDirection + Vector3.down;

            bool isWallCollision = CheckCollisionObstacle(wallDirection, _wall);
            bool isGroundCollision = CheckCollisionObstacle(groundDirection, _ground);

            if (isWallCollision || isGroundCollision && _isNotOnPlate)
            {
                Destroy();
            }

            _isNotOnPlate = !isGroundCollision;
        }

        private bool CheckCollisionObstacle(Vector3 direction, LayerMask obstacle)
        {
            Ray ray = new Ray(_selfTransform.position, direction);
            return Physics.Raycast(ray, _rayDistance, obstacle);
        }

        private void Destroy()
        {
            _rigidbody.isKinematic = true;
            _mover.Disable();
            _collisionCollider.enabled = false;
            _rotator.Disable();

            for (var i = 0; i < _fragments.Length; i++)
            {
                _fragments[i].isKinematic = false;
            }

            _timer.Restart();
            _timer.Play();
            _destroyEffect.Play();
            IsActivated = true;
        }

        private void OnTurnOffFragments()
        {
            for (var i = 0; i < _fragments.Length; i++)
            {
                _fragments[i].gameObject.Disable();
            }
        }

        private void OnDestroyGameObject() => Object.Destroy(gameObject);
    }
}