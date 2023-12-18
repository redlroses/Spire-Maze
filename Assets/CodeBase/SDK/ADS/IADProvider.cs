using System;

namespace CodeBase.SDK.ADS
{
    public interface IADProvider
    {
        void ShowRewardAd(Action onOpenCallback = null,
            Action onRewardedCallback = null,
            Action onCloseCallback = null,
            Action<string> onErrorCallback = null);

        void ShowInterstitialAd(Action onOpenCallback = null,
            Action<bool> onCloseCallback = null,
            Action<string> onErrorCallback = null,
            Action onOfflineCallback = null);
    }
}