using UnityEngine;
using NTC.Global.Cache;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InteractionHandler))]
    public class Jumper : MonoCache, IJumper
    {
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _durationJamp = 1;

        private Rigidbody _rigidbody;
        private InteractionHandler _interactionHandler;
        private float _expiredTime;
        private float _jumpProgress;
        private bool _isPlate = true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _interactionHandler = GetComponent<InteractionHandler>();
        }

        protected override void OnEnabled()
        {
            _interactionHandler.PlacedPlates += OnPlacedPlates;
        }

        protected override void OnDisabled()
        {
            _interactionHandler.PlacedPlates -= OnPlacedPlates;
        }

        protected override void FixedRun()
        {
            ApplyJump();
        }

        public void Jump()
        {
            if (_isPlate == false)
                return;

            enabled = true;  //Переделать проверку земли
        }

        private void ApplyJump()
        {
            _expiredTime += Time.deltaTime;

            if (_expiredTime > _durationJamp)
            {
                _expiredTime = 0;
                enabled = false;
            }

            _jumpProgress = _expiredTime / _durationJamp;
            _rigidbody.velocity += new Vector3(0, _jumpCurve.Evaluate(_jumpProgress) * _jumpForce, 0);
            _isPlate = false;
        }

        private void OnPlacedPlates() => _isPlate = true;
    }
}