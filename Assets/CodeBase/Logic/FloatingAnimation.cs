using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class FloatingAnimation : MonoCache
    {
        private const float FloatingRangeFactor = 0.01f;

        [SerializeField] private AnimationCurve _floatingCurve;
        [SerializeField] [Range(0f, 1f)] private float _floatingRange;
        [SerializeField] private float _floatingSpeed;
        [SerializeField] private float _rotationSpeed;

        private float _floatingHeight;

        private void Start() =>
            OnValidate();

        private void OnValidate() =>
            _floatingHeight = FloatingRangeFactor * _floatingRange;

        protected override void Run()
        {
            transform.Translate(
                0,
                _floatingCurve.Evaluate(Time.time * _floatingSpeed) * _floatingHeight,
                0,
                Space.World);

            transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        }
    }
}