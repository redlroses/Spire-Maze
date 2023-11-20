using System;
using Agava.WebUtility;

namespace CodeBase.Tools
{
    public class WebFocusObserver
    {
        public static event Action<bool> InBackgroundChangeEvent = _ => { };

        public static bool InBackground => WebApplication.InBackground || _isHidden;

        private static bool _isHidden;

        public WebFocusObserver()
        {
            WebApplication.InBackgroundChangeEvent += UpdateFocus;
        }

        public void UpdateFocus(bool isHidden)
        {
            if (isHidden == _isHidden)
                return;

            InBackgroundChangeEvent.Invoke(isHidden);
            _isHidden = isHidden;
        }
    }
}