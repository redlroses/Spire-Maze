using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(SphereCaster))]
    public class CustomGravityScaler : MonoCache
    {
        private const float Gravity = 9.81f;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SphereCaster _sphereCaster;
        [SerializeField] private float _currentScale = 1;
        [SerializeField] private float _defaultScale = 1;

        private void Awake()
        {
            _sphereCaster ??= Get<SphereCaster>();
            _rigidbody ??= Get<Rigidbody>();
            _rigidbody.useGravity = false;
            _currentScale = _defaultScale;
        }

        protected override void FixedRun()
        {
            _rigidbody.AddForce(Vector3.down * (Gravity * _currentScale), ForceMode.Acceleration);
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
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

        private bool IsOnGround() =>
            _sphereCaster.CastSphere(Vector3.down, Jumper.GroundCheckDistance);
    }
} 