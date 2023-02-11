using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class PlateMover<T> : MonoCache
    {
        private const int FinalTranslateValue = 1;

        [SerializeField] protected float _speed = 1f;

        private Rigidbody _rigidBody;
        private T _from;
        private T _to;
        private float _delta;

        protected Rigidbody RigidBody => _rigidBody;

        private void Awake()
        {
            _rigidBody = Get<Rigidbody>();
            enabled = false;
        }

        protected override void FixedRun()
        {
            Translate();
        }

        protected abstract T GetTransform(LiftDestinationMarker from);
        protected abstract void SetNewPosition(T from, T to, float delta);

        public void Move(LiftDestinationMarker from, LiftDestinationMarker to)
        {
            enabled = true;
            _from = GetTransform(from);
            _to = GetTransform(to);
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
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.fixedDeltaTime);
        }

        private void CheckIsComplete()
        {
            if (_delta >= 1)
            {
                enabled = false;
            }
        }
    }
}