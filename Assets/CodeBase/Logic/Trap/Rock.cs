using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Movement;
using CodeBase.Tools;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(RockMover))]
    [RequireComponent(typeof(RayDirection))]
    public class Rock : Trap
    {
        [SerializeField] private SphereCollider _collisionCollider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Mover _mover;
        [SerializeField] private Rigidbody[] _fragments;
        [SerializeField] private RayDirection _rayDirection;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _ground;
        [SerializeField] private ParticleSystem _destroyEffect;
        [SerializeField] private ParticleStopCallback _stopEffectCallback;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _timerDelay;

        private Transform _selfTransform;
        private Quaternion _lookRotation;
        private MoveDirection _direction;
        private bool _isActivated;
        private bool _isNotOnPlate;
        private bool _isDestroyed;
        private bool _hasTargetRotationReached;

        protected override void FixedRun()
        {
            TryDestroy();
            Rotate();
        }

        public override void Construct(TrapActivator activator)
        {
            base.Construct(Activator);
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

        protected override void Activate(IDamagable damagable)
        {
            _mover.Move(_direction);
            _mover.enabled = true;
            _timer.SetUp(_timerDelay, OnTurnOffFragments);
            _stopEffectCallback.SetCallback(OnDestroyGameObject);
            _mover.enabled = true;
            _isActivated = true;
        }

        private void Rotate()
        {
            if (_hasTargetRotationReached)
            {
                return;
            }

            Vector2 direction = new Vector2((int)_direction, 0f);
            Vector3 moveDirection = direction.ToWorldDirection(transform.parent.position, Spire.DistanceToCenter);
            _lookRotation = Quaternion.LookRotation(moveDirection);

            _rigidbody.MoveRotation(_lookRotation);
            _hasTargetRotationReached = _rigidbody.rotation == _lookRotation;
        }

        private void TryDestroy()
        {
            if (_isActivated == false || _isDestroyed == true)
                return;

            Vector3 wallDirection = _rayDirection.Calculate(Spire.Position, _selfTransform.localPosition, _direction);
            Vector3 groundDirection = Vector3.down;

            bool isWallCollision = CheckCollisionObstacle(wallDirection);
            bool isGroundCollision = CheckCollisionObstacle(groundDirection);

            if (isWallCollision || isGroundCollision && _isNotOnPlate)
            {
                Destroy();
            }

            _isNotOnPlate = !isGroundCollision;
        }

        private bool CheckCollisionObstacle(Vector3 direction)
        {
            Ray ray = new Ray(_selfTransform.position, direction);
            return Physics.Raycast(ray, _rayDistance, _ground);
        }

        private void Destroy()
        {
            _rigidbody.isKinematic = true;
            _mover.enabled = false;
            _collisionCollider.enabled = false;

            for (int i = 0; i < _fragments.Length; i++)
            {
                _fragments[i].isKinematic = false;
            }

            _timer.Restart();
            _timer.Play();
            _destroyEffect.Play();
            _isDestroyed = true;
        }

        private void OnTurnOffFragments()
        {
            for (int i = 0; i < _fragments.Length; i++)
            {
                _fragments[i].gameObject.SetActive(false);
            }
        }

        private void OnDestroyGameObject() => Destroy(gameObject);
    }
}