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

        private void OnEnable() =>
            OnResolutionChanged();

        private void OnValidate()
        {
            _onScreenStick ??= GetComponent<OnScreenStick>();
            _handler ??= GetComponent<RectTransform>();
            _rootCanvasScale ??= transform.root.GetComponentInChildren<CanvasScaler>();
        }

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