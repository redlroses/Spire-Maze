using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Tools
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CustomCanvasScaler : MonoBehaviour
    {
        [SerializeField] private CanvasScaler _canvas;

        [Space] [Header("Portable")]
        [SerializeField] private Vector2 _referenceResolutionPortable;
        [SerializeField] [Range(0f, 1f)] private float _matchWidthOrHeightPortable;

        [Space] [Header("Landscape")]
        [SerializeField] private Vector2 _referenceResolutionLandscape;
        [SerializeField] [Range(0f, 1f)] private float _matchWidthOrHeightLandscape;

        private ScreenOrientation _currentOrientation = ScreenOrientation.AutoRotation;

        private ScreenOrientation CurrentOrientation
        {
            get => _currentOrientation;
            set
            {
                if (_currentOrientation.Equals(value))
                {
                    Debug.Log($"Unchanged: {value}");
                    return;
                }

                Debug.Log($"Changed: {value}");
                _currentOrientation = value;
                OnOrientationChanged();
            }
        }

        private void OnOrientationChanged()
        {
            switch (CurrentOrientation)
            {
                case ScreenOrientation.Portrait:
                    SetCanvasParameters(_referenceResolutionPortable, _matchWidthOrHeightPortable);
                    break;
                case ScreenOrientation.LandscapeLeft:
                    SetCanvasParameters(_referenceResolutionLandscape, _matchWidthOrHeightLandscape);
                    break;
                default:
                    throw new InvalidEnumArgumentException(CurrentOrientation.ToString(),
                        (int) CurrentOrientation,
                        typeof(ScreenOrientation));
            }
        }

        private void SetCanvasParameters(Vector2 resolution, float matchWidthOrHeight)
        {
            _canvas.referenceResolution = resolution;
            _canvas.matchWidthOrHeight = matchWidthOrHeight;
        }

        private void Update()
        {
            Resolution currentResolution = Screen.currentResolution;

            CurrentOrientation = Screen.width / (float) Screen.height > 1f
                ? ScreenOrientation.LandscapeLeft
                : ScreenOrientation.Portrait;

            Debug.Log(Screen.width / (float) Screen.height);
        }
    }
}
