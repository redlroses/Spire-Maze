using System;
using CodeBase.Tools;
using NaughtyAttributes;
using NTC.Global.Cache;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.DoorEnvironment
{
    public class DoorAnimator : MonoCache
    {
        [SerializeField] private Transform _door;
        [SerializeField] private Vector3 _from;
        [SerializeField] private Vector3 _to;
        [SerializeField] private AnimationCurve _curve;

        private TowardMover<Vector3> _opening;

        private void Awake()
        {
            this.Disable();
            _opening = new TowardMover<Vector3>(_from, _to, Vector3.Lerp, _curve);
        }

        public void Open()
        {
            _opening.Reset();
            this.Enable();
        }

        protected override void Run()
        {
            if (_opening.TryUpdate(Time.deltaTime, out Vector3 position))
            {
                _door.localPosition = position;
                return;
            }

            this.Disable();
        }

        [Button]
        private void CatchFrom()
        {
            _from = _door.position;
        }

        [Button]
        private void CatchTo()
        {
            _to = _door.position;
        }
    }
}