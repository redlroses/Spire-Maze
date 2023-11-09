using System;
using CodeBase.Logic;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class BarView : MonoCache
    {
        [SerializeField] protected SliderSetter _sliderSetter;

        protected IPoints points;

        private void OnDestroy()
        {
            if (points is null)
                return;

            points.Changed -= OnChanged;
        }

        public void Construct(IPoints points)
        {
            this.points = points;
            this.points.Changed += OnChanged;
            OnChanged();
        }

        protected virtual void OnChanged() =>
            _sliderSetter.SetNormalizedValue(GetNormalizedBarValue());

        private float GetNormalizedBarValue() =>
            points.CurrentPoints / (float) points.MaxPoints;
    }
}