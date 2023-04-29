using System;
using CodeBase.Services.Pause;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    [RequireComponent(typeof(LiftPlate))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class PlateMover<T> : MonoCache, IPlateMover, IPauseWatcher
    {
        private const int FinalTranslateValue = 1;

        [SerializeField] protected float _speed = 3f;
        [SerializeField] private Rigidbody _rigidBody;

        protected float Radius;

        private T _from;
        private T _to;
        private float _delta;
        private float _distance;

        private Vector3 _prevPosition;
        private Vector3 _prevRotation;
        private IPauseReactive _pauseReactive;
        private bool _isEnabled;

        public event Action<Vector3, Vector3> PositionUpdated;
        public event Action MoveEnded;

        private Vector3 DeltaPosition => _rigidBody.position - _prevPosition;
        private Vector3 DeltaRotation => _rigidBody.rotation.eulerAngles - _prevRotation;

        protected Rigidbody RigidBody => _rigidBody;

        private void Awake()
        {
            _rigidBody ??= Get<Rigidbody>();
            Vector3 parentPosition = transform.parent.position;
            Radius = new Vector2(parentPosition.x, parentPosition.z).magnitude;
            enabled = false;
        }

        private void OnDestroy()
        {
            _pauseReactive.Pause -= OnPause;
            _pauseReactive.Resume -= OnResume;
        }

        protected override void OnEnabled()
        {
            _prevRotation = _rigidBody.rotation.eulerAngles;
            _prevPosition = _rigidBody.position;
        }

        protected override void FixedRun()
        {
            Translate();
            PositionUpdated?.Invoke(DeltaPosition, DeltaRotation);
            _prevPosition = _rigidBody.position;
            _prevRotation = _rigidBody.rotation.eulerAngles;
        }

        protected abstract T GetTransform(LiftDestinationMarker from);

        protected abstract void SetNewPosition(T from, T to, float delta);

        protected abstract float GetDistance(LiftDestinationMarker from, LiftDestinationMarker to);

        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Pause += OnPause;
            _pauseReactive.Resume += OnResume;
            Debug.Log("Registred");
        }

        public void Move(LiftDestinationMarker from, LiftDestinationMarker to)
        {
            enabled = true;
            _from = GetTransform(from);
            _to = GetTransform(to);
            _distance = GetDistance(from, to);
            _delta = 0;
        }

        private void Translate()
        {
            UpdateDelta();
            SetNewPosition(_from, _to, _delta);
            CheckIsComplete();
        }

        private void UpdateDelta()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.fixedDeltaTime / _distance);
        }

        private void CheckIsComplete()
        {
            if (_delta >= FinalTranslateValue)
            {
                enabled = false;
                MoveEnded?.Invoke();
            }
        }

        private void OnResume()
        {
            enabled = _isEnabled;
        }

        private void OnPause()
        {
            _isEnabled = enabled;
            enabled = false;
        }
    }
}