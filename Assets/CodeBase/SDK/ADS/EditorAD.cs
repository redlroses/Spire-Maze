using System;
using CodeBase.Services;
using CodeBase.UI;
using CodeBase.UI.Services.Factory;

namespace CodeBase.SDK.ADS
{
    public class EditorAD : IADProvider
    {
        private readonly IUIFactory _uiFactory;

        public EditorAD()
        {
            _uiFactory = AllServices.Container.Single<IUIFactory>();
        }

        public void ShowRewardAd(Action onOpenCallback = null, Action onRewardedCallback = null, Action onCloseCallback = null,
            Action<string> onErrorCallback = null)
        {
            onOpenCallback?.Invoke();
            _uiFactory.CreateEditorRewardADPanel().GetComponent<EditorRewardADPanel>().Open(onRewardedCallback, onCloseCallback, onErrorCallback);
        }

        public void ShowInterstitialAd(Action onOpenCallback = null, Action<bool> onCloseCallback = null, Action<string> onErrorCallback = null,
            Action onOfflineCallback = null)
        {
            onOpenCallback?.Invoke();
            _uiFactory.CreateEditorInterstitialADPanel().GetComponent<EditorInterstitialADPanel>().Open(onCloseCallback, onErrorCallback);
        }
    }
}