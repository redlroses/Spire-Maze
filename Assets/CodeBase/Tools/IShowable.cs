using System;

namespace CodeBase.Tools
{
    public interface IShowable
    {
        void Show(Action onShowCallback = null);

        void ShowInstantly();
    }
}