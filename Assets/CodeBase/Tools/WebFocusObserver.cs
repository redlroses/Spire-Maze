using System;
using Agava.WebUtility;

namespace CodeBase.Tools
{
    public class WebFocusObserver
    {
        private static bool _isHidden;

        public WebFocusObserver()
        {
            WebApplication.InBackgroundChangeEvent += OnFocusChange;
        }

        public static event Action<bool> InBackgroundChangeEvent = _ => { };

        public static bool InBackground => WebApplication.InBackground || _isHidden;

        public void Unfocus() =>
            OnFocusChange(true);

        public void Focus() =>
            OnFocusChange(false);

        private void OnFocusChange(bool isHidden)
        {
            if (isHidden == _isHidden)
                return;

            InBackgroundChangeEvent.Invoke(isHidden);
            _isHidden = isHidden;
        }
    }
}