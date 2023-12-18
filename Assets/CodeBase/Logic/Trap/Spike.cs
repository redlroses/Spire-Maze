using CodeBase.Services.Pause;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public class Spike : Trap, IPauseWatcher
    {
        private const float FinalTranslateValue = 1f;

        [SerializeField] private Transform _spikes;
        [SerializeField] private AnimationCurve _movementCurve;
        [SerializeField] private float _curveMultiplier = 1f;
        [SerializeField] private float _curveSpeed = 1f;

        private float _delta;
        private bool _isEnabled;

        public void Resume() =>
            enabled = _isEnabled;

        public void Pause()
        {
            _isEnabled = enabled;
            enabled = false;
        }

        protected override void Run()
        {
            Move();
            CheckIsComplete();
        }

        protected override void Activate() =>
            enabled = true;

        private void Move()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _curveSpeed * Time.deltaTime);
            float spikeHeight = _movementCurve.Evaluate(_delta) * _curveMultiplier;
            _spikes.localPosition = _spikes.localPosition.ChangeY(spikeHeight);
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