using System;
using UnityEngine;

namespace CodeBase.Tools
{
    public class TowardMover<TValue>
    {
        private const float FinalValue = 1;

        private readonly Func<TValue, TValue, float, TValue> _lerpFunc;
        private readonly AnimationCurve _curve;

        private TValue _from;
        private TValue _to;

        private TValue _begin;
        private TValue _end;

        private float _delta;
        private bool _isForward = true;

        public TowardMover(TValue from, TValue to, Func<TValue, TValue, float, TValue> lerp, AnimationCurve curve)
        {
            _to = to;
            _from = from;
            _lerpFunc = lerp;
            _curve = curve;

            Reset();
        }

        public TowardMover(Func<TValue, TValue, float, TValue> lerp, AnimationCurve curve)
        {
            _to = default;
            _from = default;
            _lerpFunc = lerp;
            _curve = curve;

            Reset();
        }

        private bool IsComplete => _delta >= FinalValue;

        public void SetFrom(TValue from)
        {
            if (from is null)
                throw new ArgumentNullException(nameof(from));

            _from = from;
        }

        public void SetTo(TValue to)
        {
            if (to is null)
                throw new ArgumentNullException(nameof(to));

            _to = to;
        }

        public bool TryUpdate(float deltaTime, out TValue lerpValue)
        {
            _delta = Mathf.MoveTowards(_delta, FinalValue, deltaTime);
            lerpValue = _lerpFunc.Invoke(_begin, _end, _curve.Evaluate(_delta));

            bool isComplete = IsComplete;

            if (isComplete)
                lerpValue = _end;

            return !IsComplete;
        }

        public void Reset()
        {
            _begin = _from;
            _end = _to;
            _delta = 0;
            _isForward = true;
        }

        public void Switch()
        {
            (_begin, _end) = (_end, _begin);
            _delta = FinalValue - _delta;
            _isForward = !_isForward;
        }

        public void Forward()
        {
            if (_isForward == false)
                Switch();
        }

        public void Reverse()
        {
            if (_isForward)
                Switch();
        }
    }
}