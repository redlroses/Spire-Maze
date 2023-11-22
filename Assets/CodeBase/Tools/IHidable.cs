using System;

namespace CodeBase.Tools
{
    public interface IHidable
    {
        void Hide(Action onHideCallback = null);
    }
}