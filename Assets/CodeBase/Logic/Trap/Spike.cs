using CodeBase.Logic.HealthEntity;
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
        private IPauseReactive _pauseReactive;
        private bool _isEnabled;

        private void OnDestroy()
        {
            _pauseReactive.Pause -= OnPause;
            _pauseReactive.Resume -= OnResume;
        }
        
        protected override void Run()
        {
            Move();
            CheckIsComplete();
        }

        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Pause += OnPause;
            _pauseReactive.Resume += OnResume;
        }
        
        protected override void Activate(IDamagable damagable)
        {
            enabled = true;
        }

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