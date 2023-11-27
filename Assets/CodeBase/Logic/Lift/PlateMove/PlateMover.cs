using System;
using CodeBase.Services.Pause;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    [RequireComponent(typeof(LiftPlate))]
    public abstract class PlateMover<T> : MonoCache, IPlateMover, IPauseWatcher
    {
        private const int FinalTranslateValue = 1;

        [SerializeField] protected float Speed = 3f;

        [SerializeField] private Transform _selfTransform;

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

        public Transform SelfTransform => _selfTransform;

        private Vector3 DeltaPosition => _selfTransform.position - _prevPosition;
        private Vector3 DeltaRotation => _selfTransform.rotation.eulerAngles - _prevRotation;

        private void Awake()
        {
            Vector3 parentPosition = transform.parent.position;
            Radius = new Vector2(parentPosition.x, parentPosition.z).magnitude;
            enabled = false;
        }

        protected override void OnEnabled()
        {
            _prevRotation = _selfTransform.rotation.eulerAngles;
            _prevPosition = _selfTransform.position;
        }

        protected override void Run()
        {
            _prevPosition = _selfTransform.position;
            _prevRotation = _selfTransform.rotation.eulerAngles;
            Translate();
            PositionUpdated.Invoke(DeltaPosition, DeltaRotation);
        }

        protected abstract T GetTransform(LiftDestinationMarker from);

        protected abstract void SetNewPosition(T from, T to, float delta);

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
            SetNewPosition(_from, _to, _delta);
            CheckIsComplete();
        }

        private void UpdateDelta() =>
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, Speed * Time.deltaTime / _distance);

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