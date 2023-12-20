using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.UI.Elements.Buttons
{
    public class TutorialButton : ButtonObserver
    {
        [SerializeField] private PauseToggle _pauseToggle;

        private IWindowService _windowService;

        private void OnDestroy() =>
            Cleanup();

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
            Subscribe();
        }

        protected override void OnCall()
        {
            _windowService.Open(WindowId.Tutorial);
            _pauseToggle.EmulateClick();
        }
    }
}