using CodeBase.Tools;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class SliderSetter : MonoCache
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private bool _isAnimated;
        [SerializeField] [ShowIf(nameof(_isAnimated))] private float _animationSpeed = 1f;
        
        [SerializeField] [ShowIf(nameof(_isAnimated))] [CurveRange(0, 0, 1f, 1f, EColor.Blue)]
        private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1f, 1f);

#if UNITY_EDITOR
        [SerializeField] [Range(0, 1f)] private float _testValue;
#endif

        private float _deltaAnimation;
        private float _endAnimationTime;
        private float _startAnimationValue;
        private float _endAnimationValue;
        //  private CurveAnimation _curveAnimation;
        private TowardMover<float> _animation;

        private void Awake()
        {
            enabled = false;
            //   _curveAnimation = new CurveAnimation(_curve, _animationSpeed,() => enabled = false);
            _animation = new TowardMover<float>(Mathf.Lerp, _curve);
        }

        private void OnValidate()
        {
            _curve ??= AnimationCurve.Linear(0, 0, 1f, 1f);
            //   _curveAnimation ??= new CurveAnimation(_curve, _animationSpeed, () => enabled = false);
        }

        protected override void Run()
        {
            bool isProcess = _animation.TryUpdate(Time.deltaTime * _animationSpeed, out float currentValue);
            _slider.SetValueWithoutNotify(currentValue);

            if (isProcess == false)
                enabled = false;
        }

        public void SetNormalizedValue(float value)
        {
            value = Clamp(value);
            ApplyValue(value);
        }

        public void SetNormalizedValueImmediately(float value)
        {
            value = Clamp(value);
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
            //  _curveAnimation.StartAnimation(_slider.value, endValue);
            _animation.SetFrom(_slider.value);
            _animation.SetTo(endValue);
            _animation.Reset();
            enabled = true;
        }

        private float Clamp(float value) =>
            Mathf.Clamp01(value);

#if UNITY_EDITOR
        [Button("TestSet", EButtonEnableMode.Playmode)]
        private void TestSet() =>
            SetNormalizedValue(_testValue);
#endif
    }
}