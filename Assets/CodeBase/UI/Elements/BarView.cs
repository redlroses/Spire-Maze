using CodeBase.Logic;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class BarView : MonoBehaviour
    {
        [SerializeField] protected SliderSetter _sliderSetter;

        protected IPoints Points;

        private void OnDestroy()
        {
            if (Points is null)
                return;

            Points.Changed -= OnChanged;
        }

        public void Construct(IPoints points)
        {
            Points = points;
            Points.Changed += OnChanged;
            OnChanged();
        }

        protected virtual void OnChanged() =>
            _sliderSetter.SetNormalizedValue(GetNormalizedBarValue());

        private float GetNormalizedBarValue() =>
            Points.CurrentPoints / (float) Points.MaxPoints;
    }
}