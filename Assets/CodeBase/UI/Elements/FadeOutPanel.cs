using System;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class FadeOutPanel : ShowHide<float>, IShowHide
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _initialFade;

        protected override Func<float, float, float, float> GetLerpFunction() =>
            Mathf.Lerp;

        protected override void ApplyLerpValue(float lerpValue) =>
            _canvasGroup.alpha = lerpValue;

        protected override void OnInitialize() =>
            _canvasGroup.alpha = _initialFade;
    }
}