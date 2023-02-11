using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public abstract class PlateMover : MonoCache
    {
        protected const int FinalTranslateValue = 1;

        [SerializeField] protected float _speed = 1f;

        protected Rigidbody _rigidBody;
        protected Quaternion _from;
        protected Quaternion _to;
        protected float _delta;

        private void Awake()
        {
            _rigidBody = Get<Rigidbody>();
            enabled = false;
        }

        protected override void FixedRun()
        {
            Translate();
        }

        protected abstract void Translate();

        public void Move(LiftDestinationMarker from, LiftDestinationMarker to)
        {
            enabled = true;
            _from = GetQuaternion(@from);
            _to = GetQuaternion(to);
            _delta = 0;
        }

        private Quaternion GetQuaternion(LiftDestinationMarker from) =>
            Quaternion.Euler(0, @from.Position.Angle, 0);

        protected void CheckIsComplete()
        {
            if (_delta >= 1)
            {
                enabled = false;
            }
        }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class PlateHorizontalMover : PlateMover
    {
        protected override void Translate()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.fixedDeltaTime);
            _rigidBody.rotation = Quaternion.Lerp(_from, _to, _delta);
            CheckIsComplete();
        }
    }

    public class PlateVerticalMover : PlateMover
    {
        protected override void Translate()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.fixedDeltaTime);
            _rigidBody.rotation = Quaternion.Lerp(_from, _to, _delta);
            CheckIsComplete();
        }
    }
}