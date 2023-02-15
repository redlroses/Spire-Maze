using UnityEngine;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Jumper : MonoCache, IJumper
    {
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _durationJamp = 1;
        [SerializeField] private LayerMask _ground;

        private const float OffsetYForOverlap = 0.7f;
        private const float RadiusOverlap = 0.1f;

        private Rigidbody _rigidbody;
        private float _expiredTime;
        private float _jumpProgress;
        private bool _isJump = true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected override void FixedRun()
        {
            ApplyJump();
        }

        public void Jump()
        {
            enabled = true;

            if (_isJump == true)
                return;

            Vector3 centerOverlapBox = new Vector3(transform.position.x, transform.position.y - OffsetYForOverlap, transform.position.z);
            Collider[] hits = Physics.OverlapSphere(centerOverlapBox, RadiusOverlap, _ground);

            if (hits.Length <= 0)
                enabled = false;
        }

        private void ApplyJump()
        {
            _isJump = true;
            _expiredTime += Time.fixedDeltaTime;

            if (_expiredTime > _durationJamp)
            {
                _expiredTime = 0;
                _isJump = false;
                enabled = false;
            }

            _jumpProgress = _expiredTime / _durationJamp;
            _rigidbody.velocity += new Vector3(0, _jumpCurve.Evaluate(_jumpProgress) * _jumpForce, 0);
        }
    }
}