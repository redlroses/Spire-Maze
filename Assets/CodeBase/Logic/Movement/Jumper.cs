using CodeBase.Logic.Player;
using CodeBase.Logic.StaminaEntity;
using CodeBase.Services.Pause;
using CodeBase.Tools.Extension;
using UnityEngine;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(SphereCaster))]
    [RequireComponent(typeof(CustomGravityScaler))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(HeroAnimator))]
    public class Jumper : MonoCache, IJumper,IPauseWatcher
    {
        public const float GroundCheckDistance = -0.1f;
        public const float RoofCheckDistance = 0.15f;

        private const float RadiusReduction = 0.3f;

        [SerializeField] private CustomGravityScaler _gravityScaler;
        [SerializeField] private SphereCaster _sphereCaster;
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _maxDownVelocity;
        [SerializeField] private float _maxUpVelocity;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _velocityScale;
        [SerializeField] private float _jumpDuration = 1;
        [SerializeField] private PlayerStamina _stamina;
        [SerializeField] private int _fatigue;

        private Rigidbody _rigidbody;
        private Vector3 _currentVelocity;
        private float _startHeight;
        private float _expiredTime;
        private float _jumpProgress;
        private bool _isJump;
        private IPauseReactive _pauseReactive;
        private bool _isEnabled;

        private void Awake()
        {
            _gravityScaler ??= Get<CustomGravityScaler>();
            _sphereCaster ??= Get<SphereCaster>();
            _rigidbody ??= Get<Rigidbody>();
            _heroAnimator ??= Get<HeroAnimator>();
        }

        private void OnDestroy()
        {
            _pauseReactive.Pause -= OnPause;
            _pauseReactive.Resume -= OnResume;
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

            if (isInGround == false || _stamina.TrySpend(_fatigue) == false)
            {
                return;
            }

            enabled = true;
            _heroAnimator.PlayJump();
            _gravityScaler.Disable();
        }

        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Pause += OnPause;
            _pauseReactive.Resume += OnResume;
        }

        private void OnResume()
        {
            _rigidbody.velocity = _currentVelocity;
            enabled = _isEnabled;
        }

        private void OnPause()
        {
            _currentVelocity = _rigidbody.velocity;
            _rigidbody.velocity = Vector3.zero;
            _isEnabled = enabled;
            enabled = false;
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

            bool isInRoof = _sphereCaster.CastSphere(Vector3.up, RoofCheckDistance, RadiusReduction);

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
            _gravityScaler.Enable();
        }

        private void VelocityJump()
        {
            float expectedHeight = _jumpCurve.Evaluate(_jumpProgress) * _jumpHeight + _startHeight;
            float heightDifference = expectedHeight - _rigidbody.position.y;
            float clampedVerticalVelocity = Mathf.Clamp(heightDifference * _velocityScale, -_maxDownVelocity, _maxUpVelocity);
            _rigidbody.velocity = _rigidbody.velocity.ChangeY(clampedVerticalVelocity);
        }
    }
}