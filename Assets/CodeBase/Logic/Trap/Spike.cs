using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public class Spike : Trap
    {
        private const float FinalTranslateValue = 1f;

        [SerializeField] private Transform _spikes;
        [SerializeField] private AnimationCurve _movementCurve;
        [SerializeField] private float _curveMultiplier = 1f;
        [SerializeField] private float _curveSpeed = 1f;

        private float _delta;

        protected override void Run()
        {
            Move();
            CheckIsComplete();
        }

        protected override void Activate(IDamagable damagable)
        {
            enabled = true;
        }

        private void Move()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _curveSpeed * Time.deltaTime);
            float spikeHeight = _movementCurve.Evaluate(_delta) * _curveMultiplier;
            _spikes.localPosition = _spikes.localPosition.ChangeZ(spikeHeight);
        }

        private void CheckIsComplete()
        {
            if (_delta >= FinalTranslateValue)
            {
                enabled = false;
                _delta = 0;
            }
        }
    }
}