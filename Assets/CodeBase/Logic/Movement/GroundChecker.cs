﻿using System;
using CodeBase.Services.Pause;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(SphereCaster))]
    public class GroundChecker : MonoCache, IPauseWatcher
    {
        [SerializeField] private SphereCaster _sphereCaster;

        [Space]
        [SerializeField] [Label("EnableCoyoteTime")] private bool _isEnableCoyoteTime;
        [SerializeField] [ShowIf(nameof(_isEnableCoyoteTime))] private float _coyoteTimeDuration;

        private float _coyoteTime;
        private Action _groundCheckAction;

        [ShowNativeProperty] public GroundState State { get; private set; } = GroundState.Grounded;

        private void Awake()
        {
            _sphereCaster ??= GetComponent<SphereCaster>();

            _groundCheckAction = _isEnableCoyoteTime ? CheckGroundWithCoyoteTime : CheckGround;
        }

        public void Enable() =>
            enabled = true;

        public void Disable()
        {
            enabled = false;
            State = GroundState.InAir;
        }

        public void Resume() =>
            enabled = true;

        public void Pause() =>
            enabled = false;

        protected override void FixedRun() =>
            _groundCheckAction.Invoke();

        private void CheckGroundWithCoyoteTime()
        {
            bool isTouchGround = IsTouchGround();

            if (IsGroundLost(isTouchGround))
            {
                if (_coyoteTime <= _coyoteTimeDuration)
                {
                    _coyoteTime += Time.fixedDeltaTime;

                    return;
                }
            }

            _coyoteTime = 0;
            UpdateState(isTouchGround);
        }

        private void CheckGround() =>
            UpdateState(IsTouchGround());

        private void UpdateState(bool isTouchGround) =>
            State = isTouchGround ? GroundState.Grounded : GroundState.InAir;

        private bool IsGroundLost(bool isTouchGround) =>
            isTouchGround == false && State == GroundState.Grounded;

        private bool IsTouchGround() =>
            _sphereCaster.CastSphere(Vector3.down, Jumper.GroundCheckDistance);
    }
}