﻿using System;
using CodeBase.Services.Pause;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    [RequireComponent(typeof(LiftPlate))]
    public abstract class PlateMover<T> : MonoCache, IPlateMover, IPauseWatcher
    {
        private const int FinalTranslateValue = 1;

        [SerializeField] private float _speed = 3f;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private AnimationCurve _easeInOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        protected float Radius;

        private T _from;
        private T _to;
        private float _delta;
        private float _distance;

        private Vector3 _prevPosition;
        private Vector3 _prevRotation;
        private bool _isEnabled;

        public event Action<Vector3, Vector3> PositionUpdated = (_, _) => { };
        public event Action MoveEnded = () => { };

        public float Velocity => _rigidbody.velocity.magnitude;

        protected Vector3 Position => Rigidbody.position;
        private Vector3 Rotation => Rigidbody.rotation.eulerAngles;
        protected Rigidbody Rigidbody => _rigidbody;

        private Vector3 DeltaPosition => Position - _prevPosition;
        private Vector3 DeltaRotation => Rotation - _prevRotation;

        private void Awake()
        {
            Vector3 parentPosition = transform.parent.position;
            Radius = new Vector2(parentPosition.x, parentPosition.z).magnitude;
            enabled = false;
        }

        protected override void OnEnabled()
        {
            _prevPosition = Position;
            _prevRotation = Rotation;
        }

        protected override void FixedRun()
        {
            Translate();
            PositionUpdated.Invoke(DeltaPosition, DeltaRotation);
            _prevPosition = Position;
            _prevRotation = Rotation;
        }

        protected abstract T GetTransform(LiftDestinationMarker from);

        protected abstract void UpdatePosition(T from, T to, float delta);

        protected abstract float GetDistance(LiftDestinationMarker from, LiftDestinationMarker to);

        public void Move(LiftDestinationMarker from, LiftDestinationMarker to)
        {
            enabled = true;
            _from = GetTransform(from);
            _to = GetTransform(to);
            _distance = GetDistance(from, to);
            _delta = 0;
        }

        public void Resume() =>
            enabled = _isEnabled;

        public void Pause()
        {
            _isEnabled = enabled;
            enabled = false;
        }

        private void Translate()
        {
            UpdateDelta();
            UpdatePosition(_from, _to, _easeInOutCurve.Evaluate(_delta));
            CheckIsComplete();
        }

        private void UpdateDelta()
        {
            float maxDelta = _speed * Time.fixedDeltaTime / _distance;
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, maxDelta);
        }

        private void CheckIsComplete()
        {
            if (_delta >= FinalTranslateValue)
            {
                enabled = false;
                MoveEnded.Invoke();
            }
        }
    }
}