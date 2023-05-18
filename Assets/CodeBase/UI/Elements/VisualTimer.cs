using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class VisualTimer : MonoCache
    {
        [SerializeField] private Image _clock;

        private float _reloadTime;
        private float _amountTime;

        private void Update()
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

        public void SetUp(float reloadTime) =>
            _reloadTime = reloadTime;

        public void StartReload()
        {
            _amountTime = _reloadTime;
            enabled = true;
        }
    }
}