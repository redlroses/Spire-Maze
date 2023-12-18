using System;
using CodeBase.Tools;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class FadeOutPanel : ShowHide<float>, IShowHide
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [Space]
        [SerializeField] [BoxGroup("Initial Settings")]
        private float _initialFade;
        [SerializeField] [BoxGroup("Initial Settings")] [ShowIf(nameof(_isControlInteractable))]
        private bool _initialInteractable;
        [SerializeField] [BoxGroup("Initial Settings")] [ShowIf(nameof(_isControlRaycastBlockInteractable))]
        private bool _initialRaycastBlock;

        [Space]
        [SerializeField] [BoxGroup("CanvasGroupControl")]
        private bool _isControlInteractable;
        [SerializeField] [BoxGroup("CanvasGroupControl")]
        private bool _isControlRaycastBlockInteractable;

        private void OnValidate() =>
            _canvasGroup ??= GetComponent<CanvasGroup>();

        protected override void OnInitialize()
        {
            _canvasGroup.alpha = _initialFade;

            if (_isControlInteractable)
                _canvasGroup.interactable = _initialInteractable;

            if (_isControlRaycastBlockInteractable)
                _canvasGroup.blocksRaycasts = _initialRaycastBlock;
        }

        protected override void OnShow()
        {
            if (_isControlInteractable)
                _canvasGroup.interactable = !_canvasGroup.interactable;

            if (_isControlRaycastBlockInteractable)
                _canvasGroup.blocksRaycasts = !_canvasGroup.blocksRaycasts;
        }

        protected override void OnHide()
        {
            if (_isControlInteractable)
                _canvasGroup.interactable = !_canvasGroup.interactable;

            if (_isControlRaycastBlockInteractable)
                _canvasGroup.blocksRaycasts = !_canvasGroup.blocksRaycasts;
        }

        protected override Func<float, float, float, float> GetLerpFunction() =>
            Mathf.Lerp;

        protected override void ApplyLerpValue(float lerpValue) =>
            _canvasGroup.alpha = lerpValue;
    }
}