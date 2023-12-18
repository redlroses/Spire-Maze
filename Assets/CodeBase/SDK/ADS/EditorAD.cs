using System;
using CodeBase.Services;
using CodeBase.UI;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.SDK.ADS
{
    public class EditorAD : IADProvider
    {
        private IUIFactory _uiFactory;

        public EditorAD()
        {
            _uiFactory = AllServices.Container.Single<IUIFactory>();
        }

        public void ShowRewardAd(Action onOpenCallback = null,
            Action onRewardedCallback = null,
            Action onCloseCallback = null,
            Action<string> onErrorCallback = null)
        {
            _uiFactory ??= AllServices.Container.Single<IUIFactory>();

            onOpenCallback?.Invoke();
            GameObject obj = _uiFactory.CreateEditorRewardADPanel();
            obj.GetComponent<EditorRewardADPanel>().Open(onRewardedCallback, onCloseCallback, onErrorCallback);
        }

        public void ShowInterstitialAd(Action onOpenCallback = null,
            Action<bool> onCloseCallback = null,
            Action<string> onErrorCallback = null,
            Action onOfflineCallback = null)
        {
            _uiFactory ??= AllServices.Container.Single<IUIFactory>();

            onOpenCallback?.Invoke();
            GameObject obj = _uiFactory.CreateEditorInterstitialADPanel();
            obj.GetComponent<EditorInterstitialADPanel>().Open(onCloseCallback, onErrorCallback);
        }
    }
}