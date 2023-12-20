using System;
using Agava.YandexGames;

namespace CodeBase.SDK.ADS
{
    public class YandexAD : IADProvider
    {
        public void ShowRewardAd(
            Action onOpenCallback = null,
            Action onRewardedCallback = null,
            Action onCloseCallback = null,
            Action<string> onErrorCallback = null)
        {
            VideoAd.Show(onOpenCallback, onRewardedCallback, onCloseCallback, onErrorCallback);
        }

        public void ShowInterstitialAd(
            Action onOpenCallback = null,
            Action<bool> onCloseCallback = null,
            Action<string> onErrorCallback = null,
            Action onOfflineCallback = null)
        {
            InterstitialAd.Show(onOpenCallback, onCloseCallback, onErrorCallback, onOfflineCallback);
        }
    }
}