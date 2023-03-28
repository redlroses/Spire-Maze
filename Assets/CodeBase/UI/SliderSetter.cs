using System;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class SliderSetter : MonoCache
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private bool _isAnimated;

        [SerializeField] [ShowIf(nameof(_isAnimated))]
        private float _animationSpeed;

        [SerializeField] [ShowIf(nameof(_isAnimated))]
        private AnimationCurve _curve;

#if UNITY_EDITOR
        [SerializeField] [Range(0, 1f)] private float _testValue;
#endif

        private float _deltaAnimation;
        private float _endAnimationTime;
        private float _startAnimationValue;
        private float _endAnimationValue;

        private CurveAnimation _curveAnimation;

        private void Awake()
        {
            enabled = false;
            _curveAnimation = new CurveAnimation(_curve, _animationSpeed, () => enabled = false);
        }

        private void Update()
        {
            _slider.SetValueWithoutNotify(_curveAnimation.Update(Time.deltaTime));
        }

        private void OnValidate()
        {
            _curveAnimation = new CurveAnimation(_curve, _animationSpeed, () => enabled = false);
        }

        public void SetValue(float value)
        {
            Validate(value);
            ApplyValue(value);
        }

        public void SetValueImmediately(float value)
        {
            Validate(value);
            _slider.SetValueWithoutNotify(value);
        }

        private void ApplyValue(float value)
        {
            if (_isAnimated)
            {
                StartAnimation(value);
                return;
            }

            _slider.SetValueWithoutNotify(value);
        }

        private void StartAnimation(float endValue)
        {
            _curveAnimation.StartAnimation(_slider.value, endValue);
            enabled = true;
        }

        private void Validate(float value)
        {
            if (value < _slider.minValue || value > _slider.maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

#if UNITY_EDITOR
        [Button("TestSet", EButtonEnableMode.Playmode)]
        private void TestSet()
        {
            SetValue(_testValue);
        }
#endif
    }
}