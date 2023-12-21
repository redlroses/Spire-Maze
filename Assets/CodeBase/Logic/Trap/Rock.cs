using System;
using System.Diagnostics.CodeAnalysis;
using AYellowpaper;
using CodeBase.Data;
using CodeBase.Logic.HealthEntity.Damage;
using CodeBase.Logic.Movement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools.Extension;
using NaughtyAttributes;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
    [RequireComponent(typeof(Mover))]
    public class Rock : Trap, ISavedProgress, IIndexable
    {
        [Space] [Header("Fragments")]
        [SerializeField] private Rigidbody[] _rockFragments;
        [SerializeField] private Rigidbody[] _leftWallFragments;
        [SerializeField] private Rigidbody[] _rightWallFragments;

        [Space] [Header("References")]
        [SerializeField] private CapsuleCollider _collisionCollider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Mover _mover;
        [SerializeField] private SimpleBallRotator _rotator;
        [SerializeField] private InterfaceReference<IDamageTrigger, MonoBehaviour> _damageTrigger;
        [SerializeField] private ParticleStopStateObserver _stopEffectStateObserver;
        [SerializeField] private TimerOperator _fragmentsLifetimeTimer;

        [Space] [Header("Settings")]
        [SerializeField] private float _rayDistance;
        [SerializeField] [Label("Ground Layer")] private LayerMask _ground;
        [SerializeField] private float _fragmentsLifetime = 1f;

        private Transform _selfTransform;
        private MoveDirection _moveDirection;
        private bool _isFalling;
        private bool _isTargetRotationReached;
        private float _radius;

        public event Action Destroyed = () => { };

        public int Id { get; private set; }

        public bool IsActivated { get; private set; }

        public void Construct(int id, TrapActivator activator)
        {
            Construct(activator);
            Id = id;
            _rigidbody ??= GetComponent<Rigidbody>();
            _mover ??= GetComponent<Mover>();
            _mover.enabled = false;
            _selfTransform = transform;
            _radius = _rigidbody.position.RemoveY().magnitude;
        }

        public void SetMoveDirection(bool isDirectionToRight)
        {
            _moveDirection = isDirectionToRight ? MoveDirection.Right : MoveDirection.Left;
            Rotate();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables
                .Find(cell => cell.Id == Id);

            if (cellState == null || cellState.IsActivated == false)
                return;

            IsActivated = cellState.IsActivated;

            if (cellState.IsActivated)
            {
                TurnOff(_moveDirection == MoveDirection.Left ? _leftWallFragments : _rightWallFragments);
                DestroyRock();
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables
                .Find(cell => cell.Id == Id);

            if (cellState == null)
                progress.WorldData.LevelState.Indexables.Add(new IndexableState(Id, IsActivated));
            else
                cellState.IsActivated = IsActivated;
        }

        protected override void FixedRun()
        {
            TryCollapse();
            Rotate();
        }

        protected override void OnActivate()
        {
            this.Enable();
            _mover.Move(_moveDirection);
            _mover.Enable();
            _rotator.Enable();
            _stopEffectStateObserver.SetCallback(DestroyRock);
            IsActivated = true;
            DisableKinematic(_moveDirection == MoveDirection.Left ? _leftWallFragments : _rightWallFragments);

            _fragmentsLifetimeTimer.SetUp(
                _fragmentsLifetime,
                () =>
                {
                    TurnOff(_moveDirection == MoveDirection.Left ? _leftWallFragments : _rightWallFragments);
                    TurnOff(_rockFragments);
                });
        }

        private void Rotate()
        {
            Vector2 direction = new Vector2((int)_moveDirection, 0f);
            Vector3 moveDirection = direction.ToWorldDirection(transform.parent.position, _radius);
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);

            _selfTransform.parent.rotation = lookRotation;
        }

        private void TryCollapse()
        {
            if (IsActivated == false)
                return;

            Vector3 wallDirection = _selfTransform.forward;
            Vector3 groundDirection = Vector3.down;

            bool isWallCollision = CheckCollisionObstacle(wallDirection, _ground);
            bool isGroundCollision = CheckCollisionObstacle(groundDirection, _ground);

            if (isWallCollision || (isGroundCollision && _isFalling))
            {
                Destroyed.Invoke();
                Collapse();
            }

            _isFalling = !isGroundCollision;
        }

        private bool CheckCollisionObstacle(Vector3 direction, LayerMask obstacle)
        {
            Ray ray = new Ray(_selfTransform.position, direction);

            return Physics.Raycast(ray, _rayDistance, obstacle);
        }

        private void Collapse()
        {
            _rigidbody.isKinematic = true;
            _mover.Disable();
            _collisionCollider.enabled = false;
            _damageTrigger.Value.Disable();
            _rotator.Disable();
            DisableKinematic(_rockFragments);

            _fragmentsLifetimeTimer.Restart();
            _fragmentsLifetimeTimer.Play();

            IsActivated = true;
            this.Disable();
        }

        private void DisableKinematic(Rigidbody[] fragments)
        {
            for (int i = 0; i < fragments.Length; i++)
                fragments[i].isKinematic = false;
        }

        private void TurnOff(Rigidbody[] fragments)
        {
            for (int i = 0; i < fragments.Length; i++)
                fragments[i].gameObject.Disable();
        }

        private void DestroyRock() =>
            Destroy(gameObject);
    }
}