using CodeBase.Logic.HealthEntity;
using UnityEngine;

namespace CodeBase.UI
{
    public class HealthBarView : MonoBehaviour
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
            float normalizedPoints = _health.CurrentPoints / (float) _health.MaxPoints;
            _sliderSetter.SetValueNormalized(normalizedPoints);
            _textSetter.SetText($"{_health.CurrentPoints}/{_health.MaxPoints}");
        }
    }
}