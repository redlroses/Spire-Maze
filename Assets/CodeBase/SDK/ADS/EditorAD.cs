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

        public int InterstitialAdCooldownSeconds => 10;

        public void ShowRewardAd(
            Action onCompleteCallback = null,
            Action<string> onDenyCallback = null)
        {
            _uiFactory ??= AllServices.Container.Single<IUIFactory>();
            GameObject obj = _uiFactory.CreateEditorRewardADPanel();
            obj.GetComponent<EditorRewardADPanel>().Open(null, onCompleteCallback, onDenyCallback);
        }

        public void ShowInterstitialAd(
            Action onCompleteCallback = null,
            Action<string> onDenyCallback = null)
        {
            _uiFactory ??= AllServices.Container.Single<IUIFactory>();

            GameObject obj = _uiFactory.CreateEditorInterstitialADPanel();
            obj.GetComponent<EditorInterstitialADPanel>().Open(onCompleteCallback, onDenyCallback);
        }
    }
}