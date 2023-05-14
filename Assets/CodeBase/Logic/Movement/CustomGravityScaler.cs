using CodeBase.Services.Pause;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(SphereCaster))]
    public class CustomGravityScaler : MonoCache, IPauseWatcher
    {
        private const float Gravity = 9.81f;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SphereCaster _sphereCaster;
        [SerializeField] private float _currentScale = 1;
        [SerializeField] private float _defaultScale = 1;

        public GroundState State { get; private set; } = GroundState.Grounded;

        private void Awake()
        {
            _sphereCaster ??= Get<SphereCaster>();
            _rigidbody ??= Get<Rigidbody>();
            _rigidbody.useGravity = false;
            _currentScale = _defaultScale;
        }

        protected override void FixedRun()
        {
            bool isTouchGround = IsTouchGround();
            State = isTouchGround ? GroundState.Grounded : GroundState.Falling;
            _rigidbody.AddForce(Vector3.down * (Gravity * _currentScale), ForceMode.Acceleration);
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
            State = GroundState.Falling;
        }

        public void Resume()
        {
            enabled = true;
        }

        public void Pause()
        {
            enabled = false;
        }

        public void SetGravityScale(float scale)
        {
            _currentScale = scale;
        }

        public void SetDefaultGravityScale()
        {
            _currentScale = _defaultScale;
        }

        private bool IsTouchGround() =>
            _sphereCaster.CastSphere(Vector3.down, Jumper.GroundCheckDistance);
    }
}