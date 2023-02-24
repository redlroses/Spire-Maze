using CodeBase.Data;
using UnityEngine;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CustomGravityScaler))]
    [RequireComponent(typeof(Rigidbody))]
    public class Jumper : MonoCache, IJumper
    {
        private const float GroundCheckDistance = 0.15f;
        private const float RoofCheckDistance = 0.1f;

        [SerializeField] private CustomGravityScaler _gravityScaler;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _velocityScale;
        [SerializeField] private float _jumpDuration = 1;
        [SerializeField] private LayerMask _mask;

        private float _startHeight;
        private float _expiredTime;
        private float _jumpProgress;
        private bool _isJump;
        private float _colliderRadius;
        private float _colliderHeight;

        private void Awake()
        {
            _gravityScaler ??= Get<CustomGravityScaler>();
            _rigidbody ??= Get<Rigidbody>();
            CapsuleCollider capsuleCollider = Get<CapsuleCollider>();
            _colliderRadius = capsuleCollider.radius;
            _colliderHeight = capsuleCollider.height;
        }

        protected override void FixedRun()
        {
            ApplyJump();
        }

        public void Jump()
        {
            if (_isJump)
                return;

            Vector3 position = _rigidbody.position;
            _startHeight = position.y;
            bool isInGround = CastSphere(Vector3.down, GroundCheckDistance);

            if (isInGround == false)
            {
                return;
            }

            enabled = true;
            _gravityScaler.SetGravityScale(0);
        }

        private void ApplyJump()
        {
            _isJump = true;
            _expiredTime += Time.fixedDeltaTime;

            if (_expiredTime > _jumpDuration)
            {
                AbortJump();
                return;
            }

            _jumpProgress = Mathf.Clamp01(_expiredTime / _jumpDuration);

            bool isInRoof = CastSphere(Vector3.up, RoofCheckDistance);

            if (isInRoof)
            {
                AbortJump();
            }

            VelocityJump();
        }

        private void AbortJump()
        {
            _expiredTime = 0;
            _isJump = false;
            enabled = false;
            _gravityScaler.SetDefaultGravityScale();
        }

        private bool CastSphere(Vector3 direction, float distance)
        {
            bool isHit = Physics.SphereCast(_rigidbody.position, _colliderRadius - 0.1f, direction,
                out RaycastHit info, distance + _colliderHeight / 2f - _colliderRadius, _mask);
            return isHit;
        }

        private void VelocityJump()
        {
            float expectedHeight = _jumpCurve.Evaluate(_jumpProgress) * _jumpHeight + _startHeight;
            float heightDifference = expectedHeight - _rigidbody.position.y;
            _rigidbody.velocity = _rigidbody.velocity.ChangeY(heightDifference * _velocityScale);
        }
    }
}