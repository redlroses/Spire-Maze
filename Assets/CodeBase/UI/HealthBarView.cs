using CodeBase.Logic.HealthEntity;
using UnityEngine;

namespace CodeBase.UI
{
    public sealed class HealthBarView : MonoBehaviour
    {
        [SerializeField] private SliderSetter _sliderSetter;
        [SerializeField] private TextSetter _textSetter;

        private IHealthReactive _health;

        public void Construct(IHealthReactive health)
        {
            _health = health;
            _health.Changed += OnChanged;
        }

        private void OnChanged()
        {
            _sliderSetter.SetNormalizedValue(GetNormalizedBarValue());
            ApplyTextHealth();
        }

        private float GetNormalizedBarValue() =>
            _health.CurrentPoints / (float) _health.MaxPoints;

        private void ApplyTextHealth()
        {
            _textSetter.SetText($"{_health.CurrentPoints}/{_health.MaxPoints}");
        }
    }
}