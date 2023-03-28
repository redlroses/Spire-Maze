using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.HealthEntity;
using CodeBase.Services;
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
            //TODO: посмотреть как у синдикатов это реализовано
            _health = health;
            _health.Changed += OnChanged;
        }

        private void OnChanged()
        {
            float normalizedPoints = _health.CurrentPoints / (float) _health.MaxPoints;
            _sliderSetter.SetValue(normalizedPoints);
            _textSetter.SetText($"{_health.CurrentPoints}/{_health.MaxPoints}");
        }
    }
}