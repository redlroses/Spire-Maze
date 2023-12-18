using System;
using System.Diagnostics;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class TextSetterAnimated : TextSetter
    {
        private const string DefaultStaticText = "{0}";

        [SerializeField] private AnimationCurve _curveAnimation;
        [SerializeField] private float _duration;
#if UNITY_EDITOR
        [SerializeField] private int _testNumber;
#endif

        private string _staticText;
        private float _elapsedTime;
        private int _targetNumber;

        private void Awake()
        {
            enabled = false;
            _staticText = DefaultStaticText;
        }

        public void SetStaticText(string staticText) =>
            _staticText = staticText;

        public void SetTextAnimated(int number)
        {
            _targetNumber = number;
            _elapsedTime = 0;
            enabled = true;
        }

        protected override void Run()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _duration)
            {
                SetText(_targetNumber);
                enabled = false;
            }

            string text = string.Format(_staticText, GetAnimatedNumberByTime(_elapsedTime));
            SetText(text);
        }

        private int GetAnimatedNumberByTime(float time) =>
            (int)Math.Round(_curveAnimation.Evaluate(time / _duration) * _targetNumber);

#if UNITY_EDITOR
        [Conditional("UNITY_EDITOR")]
        [Button("Set Test Text")]
        private void SetTextNumber()
        {
            SetTextAnimated(_testNumber);
        }
#endif
    }
}