using System;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace CodeBase.UI
{
    public class OnScreenStickScaler : MonoBehaviour, IResolutionDependency
    {
        private const float ToNormalized = 100f;

        [SerializeField] private OnScreenStick _onScreenStick;
        [SerializeField] private RectTransform _rect;
        [SerializeField] private float _defaultMovement;

        private void OnValidate()
        {
            _onScreenStick ??= GetComponent<OnScreenStick>();
            _rect ??= GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            ScaleMovement();
        }

        public void OnResolutionChanged()
        {
            ScaleMovement();
        }

        private void ScaleMovement()
        {
            _onScreenStick.movementRange = _defaultMovement * _rect.rect.height / ToNormalized;
        }
    }
}