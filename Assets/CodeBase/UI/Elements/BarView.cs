using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class BarView<TPoints> : MonoBehaviour
        where TPoints : IPoints
    {
        [SerializeField] private SliderSetter _sliderSetter;

        protected TPoints Points { get; private set; }

        private void OnDestroy()
        {
            if (Points is null)
                return;

            Points.Changed -= OnChanged;
            OnDestroyed();
        }

        public void Construct(TPoints points)
        {
            Points = points;
            Points.Changed += OnChanged;
            OnChanged();
            OnConstruct();
        }

        protected virtual void OnConstruct()
        {
        }

        protected virtual void OnDestroyed()
        {
        }

        protected virtual void OnChanged() =>
            _sliderSetter.SetNormalizedValue(GetNormalizedBarValue());

        private float GetNormalizedBarValue() =>
            Points.CurrentPoints / (float)Points.MaxPoints;
    }
}