using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;

namespace CodeBase.UI.Elements
{
    public class CustomOnScreenButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
    {
        [FormerlySerializedAs("m_ControlPath")]
        [InputControl(layout = "Button")]
        [SerializeField] private string _controlPath;

        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SendValueToControl(0.0f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SendValueToControl(1.0f);
        }
    }
}