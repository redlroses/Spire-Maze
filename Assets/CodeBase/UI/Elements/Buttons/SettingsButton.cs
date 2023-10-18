using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;

        private IWindowService _windowService;

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
            _toggle.onValueChanged.AddListener(Call);
        }

        private void Call(bool isOn)
        {
            if (isOn)
            {
                _windowService.Open(WindowId.Settings);
            }
        }
    }
}