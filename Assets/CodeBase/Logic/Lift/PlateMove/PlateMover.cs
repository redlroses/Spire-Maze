using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    [RequireComponent(typeof(LiftPlate))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class PlateMover<T> : MonoCache, IPlateMover
    {
        private const int FinalTranslateValue = 1;

        [SerializeField] protected float _speed = 3f;

        protected float Radius;

        private Rigidbody _rigidBody;
        private T _from;
        private T _to;
        private float _delta;
        private float _distance;

        protected Rigidbody RigidBody => _rigidBody;

        private void Awake()
        {
            _rigidBody = Get<Rigidbody>();
            Radius = new Vector2(transform.parent.position.x, transform.parent.position.z).magnitude;
            enabled = false;
        }

        protected override void FixedRun()
        {
            Translate();
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
            }
        }
    }
}