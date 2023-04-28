﻿using CodeBase.Services.Pause;
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

        private GroundState _groundState = GroundState.Grounded;
        private IPauseReactive _pauseReactive;

        public GroundState State => _groundState;

        private void Awake()
        {
            _sphereCaster ??= Get<SphereCaster>();
            _rigidbody ??= Get<Rigidbody>();
            _rigidbody.useGravity = false;
            _currentScale = _defaultScale;
        }
        
        private void OnDestroy()
        {
            _pauseReactive.Pause -= OnPause;
            _pauseReactive.Resume -= OnResume;
        }

        protected override void FixedRun()
        {
            bool isTouchGround = CheckTouchGround();

            if (isTouchGround)
            {
                _groundState = GroundState.Grounded;
            }
            else
            {
                _rigidbody.AddForce(Vector3.down * (Gravity * _currentScale), ForceMode.Acceleration);
                _groundState = GroundState.Falling;
            }
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
            _groundState = GroundState.Falling;
        }

        public void SetGravityScale(float scale)
        {
            _currentScale = scale;
        }

        public void SetDefaultGravityScale()
        {
            _currentScale = _defaultScale;
        }
        
        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Pause += OnPause;
            _pauseReactive.Resume += OnResume;
        }

        private void OnResume()
        {
            enabled = true;
        }

        private void OnPause()
        {
            enabled = false;
        }

        private bool CheckTouchGround()
        {
            return _sphereCaster.CastSphere(Vector3.down, Jumper.GroundCheckDistance);
        }
    }
}