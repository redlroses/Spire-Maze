using CodeBase.Data;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Movement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools;
using CodeBase.Tools.Extension;
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
        }

        public void LoadProgress(PlayerProgress progress)
        {
            var cellState = progress.WorldData.LevelState.Indexables
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
            var cellState = progress.WorldData.LevelState.Indexables
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
            _mover.enabled = true;
            _timer.SetUp(_timerDelay, OnTurnOffFragments);
            _stopEffectCallback.SetCallback(OnDestroyGameObject);
            _mover.enabled = true;
            IsActivated = true;
        }

        private void Rotate()
        {
            if (_hasTargetRotationReached || IsActivated)
            {
                return;
            }

            var direction = new Vector2((int) _direction, 0f);
            var moveDirection = direction.ToWorldDirection(transform.parent.position, Spire.DistanceToCenter);
            var lookRotation = Quaternion.LookRotation(moveDirection);

            _rigidbody.MoveRotation(lookRotation);
            _hasTargetRotationReached = _rigidbody.rotation == lookRotation;
        }

        private void TryDestroy()
        {
            if (IsActivated == false || IsActivated)
                return;

            var wallDirection = _rayDirection.Calculate(Spire.Position, _selfTransform.position, _direction);
            var groundDirection = wallDirection + Vector3.down;

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
            var ray = new Ray(_selfTransform.position, direction);
            return Physics.Raycast(ray, _rayDistance, obstacle);
        }

        private void Destroy()
        {
            _rigidbody.isKinematic = true;
            _mover.enabled = false;
            _collisionCollider.enabled = false;

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
                _fragments[i].gameObject.SetActive(false);
            }
        }

        private void OnDestroyGameObject() => Destroy(gameObject);
    }
}