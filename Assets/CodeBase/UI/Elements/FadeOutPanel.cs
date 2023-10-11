using System;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class FadeOutPanel : ShowHide<float>, IShowHide
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        protected override Func<float, float, float, float> GetLerpFunction() =>
            Mathf.Lerp;

        protected override void ApplyLerpValue(float lerpValue) =>
            _canvasGroup.alpha = lerpValue;
    }
}