using System;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class CurveAnimation
    {
        private readonly float _endAnimationTime;
        private readonly AnimationCurve _curve;
        private readonly float _animationSpeed;
        private readonly Action _onEndCallback;

        private float _animationTime;
        private float _startAnimationValue;
        private float _endAnimationValue;
        private float _animatedValue;
        private float _valueRange;

        public CurveAnimation(AnimationCurve curve, float animationSpeed, Action onEndCallback)
        {
            _curve = curve;
            _animationSpeed = animationSpeed;
            _endAnimationTime = _curve[_curve.length - 1].time;
            _onEndCallback = onEndCallback;
        }

        public void StartAnimation(float startValue, float endValue)
        {
            _animationTime = 0;
            _startAnimationValue = startValue;
            _endAnimationValue = endValue;
            _valueRange = Mathf.Abs(endValue - startValue);
        }

        public float Update(float deltaTime)
        {
            UpdateDelta(deltaTime);
            return Translate();
        }

        private float Translate()
        {
            float animatedDelta = _curve.Evaluate(_animationTime);
            return Mathf.Lerp(_startAnimationValue, _endAnimationValue, animatedDelta);
        }

        private void UpdateDelta(float deltaTime)
        {
            _animationTime += deltaTime * _animationSpeed / _valueRange;

            if (_animationTime >= _endAnimationTime)
            {
                _animationTime = _endAnimationTime;
                _onEndCallback.Invoke();
            }
        }
    }
}