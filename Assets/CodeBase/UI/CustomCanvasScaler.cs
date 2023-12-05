#if !UNITY_EDITOR
using Agava.WebUtility;
#endif
using CodeBase.Logic.Cameras;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class CustomCanvasScaler : MonoBehaviour, IResolutionDependency
    {
        [SerializeField] private float _landscapeFactor;
        [SerializeField] private float _portraitFactor;
        [SerializeField] private CanvasScaler _canvasScaler;

#if UNITY_EDITOR
        [SerializeField] private bool _isDebugMobile;
#endif

        private void OnEnable() =>
            Scale();

        public void OnResolutionChanged() =>
            Scale();

        private void Scale() =>
            _canvasScaler.scaleFactor = IsMobileLandscape() ? _landscapeFactor : _portraitFactor;

        private bool IsMobileLandscape() =>
            IsMobile() && ResolutionMonitor.CurrentScreenConfiguration.Name.Equals(Orientations.Landscape.ToString());

#if !UNITY_EDITOR
        private bool IsMobile() =>
            Device.IsMobile;
#else
        private bool IsMobile() =>
            _isDebugMobile;
#endif
    }
}