using Agava.WebUtility;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class MobileInputPanel : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR
            TrackEditorOrientation();
#else
            TrackDeviceType();
#endif
        }

        private void TrackDeviceType()
        {
            gameObject.SetActive(Device.IsMobile);
        }

        private void TrackEditorOrientation()
        {
            Vector2 resolution = ResolutionMonitor.CurrentResolution;
            gameObject.SetActive(resolution.x < resolution.y);
        }
    }
}