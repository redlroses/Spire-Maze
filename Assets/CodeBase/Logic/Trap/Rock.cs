using CodeBase.Data;
using CodeBase.Logic.Movement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools.Extension;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(RockMover))]
    public class Rock : Trap, ISavedProgress, IIndexable
    {
        [SerializeField] private CapsuleCollider _collisionCollider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private RockMover _mover;
        [SerializeField] private SimpleBallRotator _rotator;
        [SerializeField] private Rigidbody[] _fragments;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _ground;
        [SerializeField] private LayerMask _wall;
        [SerializeField] private ParticleSystem _destroyEffect;
        [SerializeField] private ParticleStopCallback _stopEffectCallback;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _timerDelay;

        private Transform _selfTransform;
        private MoveDirection _moveDirection;
        private bool _isNotOnPlate;
        private bool _hasTargetRotationReached;
        private float _radius;

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        protected override void FixedRun()
        {
            TryDestroy();
            Rotate();
        }

        protected override void Activate()
        {
            this.Enable();
            _mover.Move(_moveDirection);
            _mover.Enable();
            _rotator.Enable();
            _timer.SetUp(_timerDelay, OnTurnOffFragments);
            _stopEffectCallback.SetCallback(OnDestroyGameObject);
            IsActivated = true;
        }

        public void Construct(int id, TrapActivator activator)
        {
            base.Construct(activator);
            Id = id;
            _rigidbody ??= GetComponent<Rigidbody>();
            _mover ??= GetComponent<RockMover>();
            _mover.enabled = false;
            _selfTransform = transform;
            _radius = _rigidbody.position.RemoveY().magnitude;
            Debug.Log(_radius + gameObject.name);
        }

        public void SetMoveDirection(bool isDirectionToRight)
        {
            _moveDirection = isDirectionToRight ? MoveDirection.Right : MoveDirection.Left;
            Rotate();
            _rotator.SetDirection(_moveDirection);
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

        private void Rotate()
        {
            Vector2 direction = new Vector2((int) _moveDirection, 0f);
            Vector3 moveDirection = direction.ToWorldDirection(transform.parent.position, _radius);
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);

            _selfTransform.parent.rotation = lookRotation;
        }

        private void TryDestroy()
        {
            if (IsActivated == false)
                return;

            Vector3 wallDirection = _selfTransform.forward;
            Vector3 groundDirection = Vector3.down;

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
            Debug.DrawRay(ray.origin, ray.direction * _rayDistance, Color.blue);
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

        private void OnDestroyGameObject() =>
            Object.Destroy(gameObject);
    }
}