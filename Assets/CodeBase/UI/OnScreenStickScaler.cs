using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class OnScreenStickScaler : MonoBehaviour, IResolutionDependency
    {
        private const float ToNormalized = 0.01f;

        [SerializeField] private OnScreenStick _onScreenStick;
        [SerializeField] private RectTransform _handler;
        [SerializeField] private CanvasScaler _rootCanvasScale;
        [SerializeField] private float _defaultMovement;
        [SerializeField] private float _power;

        private void OnValidate()
        {
            _onScreenStick ??= GetComponent<OnScreenStick>();
            _handler ??= GetComponent<RectTransform>();
            _rootCanvasScale ??= transform.root.GetComponentInChildren<CanvasScaler>();
        }

        private void OnEnable() =>
            OnResolutionChanged();

        public void OnResolutionChanged()
        {
            ScaleMovement();
            ScaleRect();
        }

        private void ScaleMovement() =>
            _onScreenStick.movementRange =
                _defaultMovement / _rootCanvasScale.scaleFactor * _handler.rect.height * ToNormalized;

        private void ScaleRect() =>
            _handler.transform.localScale = Vector3.one / _rootCanvasScale.scaleFactor;
    }
}