using System;
using System.Diagnostics;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    class TextSetterAnimated : TextSetter
    {
        [SerializeField] private AnimationCurve _curveAnimation;
        [SerializeField] private float _duration;
        
#if UNITY_EDITOR
        [SerializeField] private int _testNumber;
#endif

        private float _elapsedTime;
        private int _targetNumber;

        protected override void Run()
        {
            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime > _duration)
            {
                SetText(_targetNumber);
                enabled = false;
            }

            SetText(GetAnimatedNumberByTime(_elapsedTime));
        }
        
        public void SetTextAnimated(int number)
        {
            _targetNumber = number;
            _elapsedTime = 0;
            enabled = true;
        }

        private int GetAnimatedNumberByTime(float time) =>
            (int) Math.Round(_curveAnimation.Evaluate(time / _duration) * _targetNumber);

        [Conditional("UNITY_EDITOR")] [Button("Set Test Text")]
        private void SetTextNumber()
        {
            SetTextAnimated(_testNumber);
        }
    }
}