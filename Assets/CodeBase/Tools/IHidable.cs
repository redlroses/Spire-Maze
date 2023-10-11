using System;

namespace CodeBase.Tools
{
    public interface IHidable
    {
        void Hide(Action OnHideCallback = null);
    }
}