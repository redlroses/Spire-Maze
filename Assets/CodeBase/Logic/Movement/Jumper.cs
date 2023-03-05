using CodeBase.Data;
using CodeBase.Tools.Extension;
using UnityEngine;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(SphereCaster))]
    [RequireComponent(typeof(CustomGravityScaler))]
    [RequireComponent(typeof(Rigidbody))]
    public class Jumper : MonoCache, IJumper
    {
        public const float GroundCheckDistance = -0.15f;
        public const float RoofCheckDistance = 0.15f;

        [SerializeField] private CustomGravityScaler _gravityScaler;
        [SerializeField] private SphereCaster _sphereCaster;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _velocityScale;
        [SerializeField] private float _jumpDuration = 1;

        private Rigidbody _rigidbody;
        private float _startHeight;
        private float _expiredTime;
        private float _jumpProgress;
        private bool _isJump;

        private void Awake()
        {
            _gravityScaler ??= Get<CustomGravityScaler>();
            _sphereCaster ??= Get<SphereCaster>();
            _rigidbody ??= Get<Rigidbody>();
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
            bool isInGround = _sphereCaster.CastSphere(Vector3.down, GroundCheckDistance);

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

            bool isInRoof = _sphereCaster.CastSphere(Vector3.up, RoofCheckDistance, 0.3f);

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

        private void VelocityJump()
        {
            float expectedHeight = _jumpCurve.Evaluate(_jumpProgress) * _jumpHeight + _startHeight;
            float heightDifference = expectedHeight - _rigidbody.position.y;
            _rigidbody.velocity = _rigidbody.velocity.ChangeY(heightDifference * _velocityScale);
        }
    }
}