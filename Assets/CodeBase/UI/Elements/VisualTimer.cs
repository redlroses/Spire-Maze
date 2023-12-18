using CodeBase.Services.Pause;
using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class VisualTimer : MonoCache, IPauseWatcher
    {
        [SerializeField] private Image _clock;

        private float _reloadTime;
        private float _amountTime;

        private bool _cashedEnableState;

        public void SetUp(float reloadTime) =>
            _reloadTime = reloadTime;

        public void StartReload()
        {
            _amountTime = _reloadTime;
            enabled = true;
        }

        public void Pause()
        {
            _cashedEnableState = enabled;
            enabled = false;
        }

        public void Resume() =>
            enabled = _cashedEnableState;

        protected override void Run()
        {
            _amountTime -= Time.deltaTime;

            if (_amountTime <= 0)
                enabled = false;

            ApplyFill(_amountTime);
        }

        private void ApplyFill(float byAmountTime)
        {
            float normalizedAmountTime = Mathf.InverseLerp(0, _reloadTime, byAmountTime);
            _clock.fillAmount = normalizedAmountTime;
        }
    }
}