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

        private void OnEnable() =>
            Scale();

        public void OnResolutionChanged() =>
            Scale();

        private void Scale() =>
            _canvasScaler.scaleFactor = IsMobileLandscape() ? _landscapeFactor : _portraitFactor;

        private bool IsMobileLandscape() =>
            Application.isMobilePlatform && ResolutionMonitor.CurrentScreenConfiguration.Name.Equals(Orientations.Portrate.ToString());
    }
}