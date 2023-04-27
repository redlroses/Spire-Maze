using CodeBase.Logic.Observer;
using CodeBase.Logic.Portal;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Logic
{
    public class FinishPortal : ObserverTarget<TeleportableObserver, ITeleportable>
    {
        private IWindowService _windowService;

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        protected override void OnTriggerObserverEntered(ITeleportable target)
        {
            _windowService.Open(WindowId.Results);
        }
    }
}