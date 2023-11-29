using CodeBase.Logic.Hero;
using CodeBase.Logic.StaminaEntity;
using CodeBase.Services.Pause;
using CodeBase.Tools.Extension;
using UnityEngine;
using NTC.Global.Cache;
using NTC.Global.System;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(SphereCaster))]
    [RequireComponent(typeof(GroundChecker))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(HeroAnimator))]
    public class Jumper : MonoCache, IPauseWatcher
    {
        public const float GroundCheckDistance = -0.17f;
        public const float RoofCheckDistance = 0.15f;

        private const float CastSphereRadiusReduction = 0.3f;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ConstantForce _gravity;
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private SphereCaster _sphereCaster;
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private PlayerStamina _stamina;
        [SerializeField] private float _maxDownVelocity;
        [SerializeField] private float _maxUpVelocity;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _velocityScale;
        [SerializeField] private float _jumpDuration;
        [SerializeField] private int _staminaCosts;

        private Vector3 _currentVelocity;
        private float _startHeight;
        private float _expiredTime;
        private float _jumpProgress;
        private bool _isJump;
        private bool _isEnabled;
        private bool _isPause;

        public bool CanJump => IsJumpAvailable();

        private void OnValidate()
        {
            _groundChecker ??= GetComponent<GroundChecker>();
            _sphereCaster ??= GetComponent<SphereCaster>();
            _rigidbody ??= GetComponent<Rigidbody>();
            _heroAnimator ??= GetComponent<HeroAnimator>();
        }

        protected override void FixedRun() =>
            ProcessJump();

        public void Resume()
        {
            _rigidbody.velocity = _currentVelocity;
            enabled = _isEnabled;
            _isPause = false;
        }

        public void Pause()
        {
            _currentVelocity = _rigidbody.velocity;
            _rigidbody.velocity = Vector3.zero;
            _isEnabled = enabled;
            enabled = false;
            _isPause = true;
        }

        public void Jump()
        {
            Vector3 position = _rigidbody.position;
            _startHeight = position.y;
            enabled = true;
            _heroAnimator.PlayJump();
            DisableGravity();
        }

        private bool IsJumpAvailable()
        {
            if (_isPause)
                return false;

            if (_isJump)
                return false;

            return _groundChecker.State == GroundState.Grounded && _stamina.TrySpend(_staminaCosts);
        }

        private void ProcessJump()
        {
            _isJump = true;
            _expiredTime += Time.fixedDeltaTime;

            if (_expiredTime > _jumpDuration)
            {
                AbortJump();
                return;
            }

            _jumpProgress = Mathf.Clamp01(_expiredTime / _jumpDuration);
            bool isTouchRoof = _sphereCaster.CastSphere(Vector3.up, RoofCheckDistance, CastSphereRadiusReduction);

            if (isTouchRoof)
            {
                AbortJump();
            }

            ApplyJumpVelocity();
        }

        private void AbortJump()
        {
            _expiredTime = 0;
            _isJump = false;
            enabled = false;
            EnableGravity();
        }

        private void ApplyJumpVelocity()
        {
            float expectedHeight = _jumpCurve.Evaluate(_jumpProgress) * _jumpHeight + _startHeight;
            float heightDifference = expectedHeight - _rigidbody.position.y;
            float clampedVerticalVelocity =
                Mathf.Clamp(heightDifference * _velocityScale, -_maxDownVelocity, _maxUpVelocity);
            _rigidbody.velocity = _rigidbody.velocity.ChangeY(clampedVerticalVelocity);
        }

        private void EnableGravity()
        {
            _groundChecker.Enable();
            _gravity.enabled = true;
        }

        private void DisableGravity()
        {
            _groundChecker.Disable();
            _gravity.enabled = false;
        }
    }
}