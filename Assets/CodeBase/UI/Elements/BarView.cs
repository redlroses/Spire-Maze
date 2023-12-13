using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class BarView<TPoints> : MonoBehaviour where TPoints : IPoints
    {
        [SerializeField] protected SliderSetter _sliderSetter;

        protected TPoints Points;

        private void OnDestroy()
        {
            if (Points is null)
                return;

            Points.Changed -= OnChanged;
            OnDestroyed();
        }

        protected virtual void OnConstruct() { }

        protected virtual void OnChanged() =>
            _sliderSetter.SetNormalizedValue(GetNormalizedBarValue());

        protected virtual void OnDestroyed() { }

        public void Construct(TPoints points)
        {
            Points = points;
            Points.Changed += OnChanged;
            OnChanged();
            OnConstruct();
        }

        private float GetNormalizedBarValue() =>
            Points.CurrentPoints / (float) Points.MaxPoints;
    }
}